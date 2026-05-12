using Godot;

[GlobalClass]
public partial class CardData : Resource
{
	[Export] public string CardName { get; set; }
	[Export] public Texture2D CardArt { get; set; }
}
