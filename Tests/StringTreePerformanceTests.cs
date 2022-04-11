using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using AntiStickerBot;
using DSharpPlus.Entities;
using NUnit.Framework;

namespace Tests; 

public class StringTreePerformanceTests {
	private StringTree m_Tree = null!;
	
	[SetUp]
	public void Setup() {
		m_Tree = new StringTree();
	}

	[Test]
	public void SixtyPercentFasterThanHashSet() {
		Dictionary<string, string>.KeyCollection allEmojis = ((Dictionary<string, string>) typeof(DiscordEmoji).GetProperty("DiscordNameLookup", BindingFlags.NonPublic | BindingFlags.Static)!.GetValue(null)!).Keys;

		List<string> emojisList = allEmojis.ToList();
		
		Dictionary<int, HashSet<string>> emojiLookup = ((Dictionary<string, string>) typeof(DiscordEmoji).GetProperty("DiscordNameLookup", BindingFlags.NonPublic | BindingFlags.Static)!.GetValue(null)!).Keys
			.GroupBy(emoji => emoji.Length)
			.ToDictionary(
				grp => grp.Key,
				grp => grp.ToHashSet()
			);

		var emojiTree = new StringTree();
		foreach (string emoji in allEmojis) {
			emojiTree.Add(emoji);
		}

		var random = new Random();
		var stw = new Stopwatch();

		List<string> haystacks = new List<string>(1000);
		haystacks.AddRange(emojisList.Take(500));
		for (int i = 0; i < 500; i++) {
			char[] chars = new char[random.Next(10, 20)];
			byte[] bytes = new byte[chars.Length * 2];
			random.NextBytes(bytes);
			for (int bi = 0; bi < chars.Length; bi += 2) {
				chars[bi] = (char) ((bytes[bi] << 8) | bytes[bi + 1]);
			}

			haystacks.Add(new string(chars));
		}
		for (int i = 0; i < 10; i++) {
			stw.Start();
			foreach (string haystack in haystacks) {
				bool TestHaystack() {
					// If you have any ideas how to improve this, be my guest.
					foreach (int emojiLength in emojiLookup.Keys) {
						HashSet<string> emojisOfThisLength = emojiLookup[emojiLength];
						for (int si = 0; si < haystack.Length - emojiLength; si++) {
							if (emojisOfThisLength.Contains(haystack.Substring(si, emojiLength))) {
								return true;
							}
						}
					}

					return false;
				}

				_ = TestHaystack();
				//_ = emojisHashSet.Contains(emojisList[index]);
			}
			stw.Stop();
			TimeSpan hashSetTime = stw.Elapsed;
			stw.Reset();
			
			stw.Start();
			foreach (string haystack in haystacks) {
				ReadOnlySpan<char> span = haystack.AsSpan();
				for (int si = 0; si < haystack.Length; si++) {
					_ = emojiTree.ContainsPrefix(span.Slice(si));
				}
			}
			stw.Stop();
			TimeSpan treeTime = stw.Elapsed;
			Console.WriteLine($"HashSet time: {hashSetTime}");
			Console.WriteLine($"StringTree time: {treeTime}");
			Assert.Less(treeTime, hashSetTime * 0.40);
			stw.Reset();
		}
	}

}
