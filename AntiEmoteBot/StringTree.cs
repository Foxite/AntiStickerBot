namespace AntiStickerBot; 

public class StringTree {
	private Node m_RootNode = new();

	public bool Add(string s) => Add(s.AsSpan());
	public bool Add(ReadOnlySpan<char> s) => m_RootNode.Add(s, 0);

	public int ContainsPrefix(string s) => ContainsPrefix(s.AsSpan());
	public int ContainsPrefix(ReadOnlySpan<char> s) => m_RootNode.ContainsPrefix(s, 0);

	private class Node {
		public Dictionary<char, Node> Children { get; } = new();
		public bool IsValue { get; set; }
		
		public bool Add(ReadOnlySpan<char> span, int pos) {
			if (pos == span.Length) {
				if (IsValue) {
					return false;
				} else {
					IsValue = true;
					return true;
				}
			} else {
				if (!Children.TryGetValue(span[pos], out Node? child)) {
					child = new Node();
					Children[span[pos]] = child;
				}

				return child.Add(span, pos + 1);
			}
		}

		public int ContainsPrefix(ReadOnlySpan<char> span, int pos) {
			if (IsValue) {
				if (pos < span.Length && Children.TryGetValue(span[pos], out Node? child)) {
					var ret = child.ContainsPrefix(span, pos + 1);
					if (ret != -1) {
						return ret;
					}
				}
				return pos;
				//return Value != null && Value.AsSpan() == span;
			} else if (pos == span.Length) {
				return -1;
			} else {
				if (Children.TryGetValue(span[pos], out Node? child)) {
					return child.ContainsPrefix(span, pos + 1);
				} else {
					return -1;
				}
			}
		}
	}
}