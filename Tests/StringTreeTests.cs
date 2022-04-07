using Foxite.Common.Collections;
using NUnit.Framework;

namespace Tests;

public class StringTreeTests {
	private StringTree m_Tree = null!;
	
	[SetUp]
	public void Setup() {
		m_Tree = new StringTree();
	}

	[Test]
	public void EmptyTree() {
		Assert.AreEqual(-1, m_Tree.ContainsPrefix(""));
		Assert.AreEqual(-1, m_Tree.ContainsPrefix("B"));
		Assert.AreEqual(-1, m_Tree.ContainsPrefix("Bl"));
		Assert.AreEqual(-1, m_Tree.ContainsPrefix("Bla"));
	}

	[Test]
	public void OnlyEmptyString() {
		Assert.True(m_Tree.Add(""));
		Assert.False(m_Tree.Add(""));
		Assert.AreEqual(0, m_Tree.ContainsPrefix(""));
		Assert.AreEqual(0, m_Tree.ContainsPrefix("B"));
		Assert.AreEqual(0, m_Tree.ContainsPrefix("Bl"));
		Assert.AreEqual(0, m_Tree.ContainsPrefix("Bla"));
	}

	[Test]
	public void SingleCharacterStrings() {
		Assert.True(m_Tree.Add("B"));
		Assert.False(m_Tree.Add("B"));
		Assert.AreEqual(-1, m_Tree.ContainsPrefix(""));
		Assert.AreEqual(1, m_Tree.ContainsPrefix("B"));
		Assert.AreEqual(1, m_Tree.ContainsPrefix("Bl"));
		Assert.AreEqual(1, m_Tree.ContainsPrefix("Bla"));
		Assert.AreEqual(-1, m_Tree.ContainsPrefix("C"));
		Assert.AreEqual(-1, m_Tree.ContainsPrefix("Cl"));
		Assert.AreEqual(-1, m_Tree.ContainsPrefix("Cla"));
		Assert.AreEqual(-1, m_Tree.ContainsPrefix("D"));
		Assert.AreEqual(-1, m_Tree.ContainsPrefix("Dl"));
		Assert.AreEqual(-1, m_Tree.ContainsPrefix("Dla"));
		
		Assert.True(m_Tree.Add("C"));
		Assert.False(m_Tree.Add("C"));
		Assert.AreEqual(-1, m_Tree.ContainsPrefix(""));
		Assert.AreEqual(1, m_Tree.ContainsPrefix("B"));
		Assert.AreEqual(1, m_Tree.ContainsPrefix("Bl"));
		Assert.AreEqual(1, m_Tree.ContainsPrefix("Bla"));
		Assert.AreEqual(1, m_Tree.ContainsPrefix("C"));
		Assert.AreEqual(1, m_Tree.ContainsPrefix("Cl"));
		Assert.AreEqual(1, m_Tree.ContainsPrefix("Cla"));
		Assert.AreEqual(-1, m_Tree.ContainsPrefix("D"));
		Assert.AreEqual(-1, m_Tree.ContainsPrefix("Dl"));
		Assert.AreEqual(-1, m_Tree.ContainsPrefix("Dla"));
	}

	[Test]
	public void MultiCharacterStrings() {
		Assert.True(m_Tree.Add("Bla"));
		Assert.False(m_Tree.Add("Bla"));
		Assert.AreEqual(-1, m_Tree.ContainsPrefix(""));
		Assert.AreEqual(-1, m_Tree.ContainsPrefix("B"));
		Assert.AreEqual(-1, m_Tree.ContainsPrefix("Bl"));
		Assert.AreEqual(3, m_Tree.ContainsPrefix("Bla"));
		Assert.AreEqual(3, m_Tree.ContainsPrefix("Blad"));
		Assert.AreEqual(-1, m_Tree.ContainsPrefix("C"));
		Assert.AreEqual(-1, m_Tree.ContainsPrefix("Cl"));
		Assert.AreEqual(-1, m_Tree.ContainsPrefix("Cla"));
		Assert.AreEqual(-1, m_Tree.ContainsPrefix("Clad"));
		
		Assert.True(m_Tree.Add("Cla"));
		Assert.False(m_Tree.Add("Cla"));
		Assert.AreEqual(-1, m_Tree.ContainsPrefix(""));
		Assert.AreEqual(-1, m_Tree.ContainsPrefix("B"));
		Assert.AreEqual(-1, m_Tree.ContainsPrefix("Bl"));
		Assert.AreEqual(3, m_Tree.ContainsPrefix("Bla"));
		Assert.AreEqual(3, m_Tree.ContainsPrefix("Blad"));
		Assert.AreEqual(-1, m_Tree.ContainsPrefix("C"));
		Assert.AreEqual(-1, m_Tree.ContainsPrefix("Cl"));
		Assert.AreEqual(3, m_Tree.ContainsPrefix("Cla"));
		Assert.AreEqual(3, m_Tree.ContainsPrefix("Clad"));
	}

	[Test]
	public void ExtensionStrings() {
		Assert.True(m_Tree.Add("Bl"));
		Assert.False(m_Tree.Add("Bl"));
		Assert.AreEqual(-1, m_Tree.ContainsPrefix(""));
		Assert.AreEqual(-1, m_Tree.ContainsPrefix("B"));
		Assert.AreEqual(2, m_Tree.ContainsPrefix("Bl"));
		Assert.AreEqual(2, m_Tree.ContainsPrefix("Bla"));
		Assert.AreEqual(2, m_Tree.ContainsPrefix("Blad"));
		Assert.AreEqual(-1, m_Tree.ContainsPrefix("C"));
		Assert.AreEqual(-1, m_Tree.ContainsPrefix("Cl"));
		Assert.AreEqual(-1, m_Tree.ContainsPrefix("Cla"));
		Assert.AreEqual(-1, m_Tree.ContainsPrefix("Clad"));
		
		Assert.True(m_Tree.Add("Bla"));
		Assert.False(m_Tree.Add("Bla"));
		Assert.AreEqual(-1, m_Tree.ContainsPrefix(""));
		Assert.AreEqual(-1, m_Tree.ContainsPrefix("B"));
		Assert.AreEqual(2, m_Tree.ContainsPrefix("Bl"));
		Assert.AreEqual(3, m_Tree.ContainsPrefix("Bla"));
		Assert.AreEqual(3, m_Tree.ContainsPrefix("Blad"));
		Assert.AreEqual(-1, m_Tree.ContainsPrefix("C"));
		Assert.AreEqual(-1, m_Tree.ContainsPrefix("Cl"));
		Assert.AreEqual(-1, m_Tree.ContainsPrefix("Cla"));
		Assert.AreEqual(-1, m_Tree.ContainsPrefix("Clad"));
	}

	[Test]
	public void ExtensionStrings2() {
		Assert.True(m_Tree.Add("Bla"));
		Assert.False(m_Tree.Add("Bla"));
		Assert.AreEqual(-1, m_Tree.ContainsPrefix(""));
		Assert.AreEqual(-1, m_Tree.ContainsPrefix("B"));
		Assert.AreEqual(-1, m_Tree.ContainsPrefix("Bl"));
		Assert.AreEqual(3, m_Tree.ContainsPrefix("Bla"));
		Assert.AreEqual(3, m_Tree.ContainsPrefix("Blad"));
		Assert.AreEqual(-1, m_Tree.ContainsPrefix("C"));
		Assert.AreEqual(-1, m_Tree.ContainsPrefix("Cl"));
		Assert.AreEqual(-1, m_Tree.ContainsPrefix("Cla"));
		Assert.AreEqual(-1, m_Tree.ContainsPrefix("Clad"));
		
		Assert.True(m_Tree.Add("Bl"));
		Assert.False(m_Tree.Add("Bl"));
		Assert.AreEqual(-1, m_Tree.ContainsPrefix(""));
		Assert.AreEqual(-1, m_Tree.ContainsPrefix("B"));
		Assert.AreEqual(2, m_Tree.ContainsPrefix("Bl"));
		Assert.AreEqual(3, m_Tree.ContainsPrefix("Bla"));
		Assert.AreEqual(3, m_Tree.ContainsPrefix("Blad"));
		Assert.AreEqual(-1, m_Tree.ContainsPrefix("C"));
		Assert.AreEqual(-1, m_Tree.ContainsPrefix("Cl"));
		Assert.AreEqual(-1, m_Tree.ContainsPrefix("Cla"));
		Assert.AreEqual(-1, m_Tree.ContainsPrefix("Clad"));
	}
}
