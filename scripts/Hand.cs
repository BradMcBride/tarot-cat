using Godot;
using System.Collections.Generic;

public partial class Hand : Control
{
	[Export] public PackedScene CardScene;
	[Export] public float Spread = 40.0f;
	[Export] public float HSpacing = 120.0f;
	[Export] public float ArcHeight = 20.0f;

	private List<Card> _cards = new();

	public override void _Ready()
	{
		// Test with empty cards, replace with real data later
		for (int i = 0; i < 5; i++)
			AddCard(null);
	}

	public void AddCard(CardData data)
	{
		var card = CardScene.Instantiate<Card>();
		AddChild(card);

		if (data != null)
			card.ApplyData(data);

		_cards.Add(card);
		ArrangeHand();
	}

	public void RemoveCard(Card card)
	{
		_cards.Remove(card);
		card.QueueFree();
		ArrangeHand();
	}

	private void ArrangeHand()
	{
		int count = _cards.Count;
		if (count == 0) return;

		for (int i = 0; i < count; i++)
		{
			float t = count > 1 ? (float)i / (count - 1) : 0.5f;

			float x = Mathf.Lerp(
				-HSpacing * (count - 1) / 2,
				 HSpacing * (count - 1) / 2, t);

			// Parabola — edges dip down, center sits higher
			float arc = ArcHeight * (4 * (t - 0.5f) * (t - 0.5f));

			float rotation = Mathf.Lerp(-Spread / 2, Spread / 2, t);

			var card = _cards[i];
			card.Position = new Vector2(x, arc);
			card.RotationDegrees = rotation;
			card.Scale = Vector2.One;
			card.ZIndex = i;

			card.StoreOriginalTransform();
		}
	}
}
