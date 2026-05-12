using Godot;

public partial class Card : Control
{
	[Export] public CardData Data;

	private Vector2 _originalPosition;
	private float _originalRotation;
	private Tween _tween;

	private TextureRect _art;

	public override void _Ready()
	{
		_art = GetNode<TextureRect>("TextureRect");

		if (Data != null)
			ApplyData(Data);

		MouseFilter = MouseFilterEnum.Stop;
		PivotOffset = Size / 2;
		MouseEntered += OnMouseEntered;
		MouseExited += OnMouseExited;
	}

	public void ApplyData(CardData data)
	{
		Data = data;
		_art.Texture = data.CardArt;
	}

	public void StoreOriginalTransform()
	{
		_originalPosition = Position;
		_originalRotation = RotationDegrees;
	}

	private void OnMouseEntered()
	{
		_tween?.Kill();
		_tween = CreateTween().SetParallel(true);
		_tween.TweenProperty(this, "scale", new Vector2(1.3f, 1.3f), 0.15);
		_tween.TweenProperty(this, "position:y", _originalPosition.Y - 40, 0.15);
		_tween.TweenProperty(this, "rotation_degrees", 0.0f, 0.15);
		ZIndex = 10;
	}

	private void OnMouseExited()
	{
		_tween?.Kill();
		_tween = CreateTween().SetParallel(true);
		_tween.TweenProperty(this, "scale", Vector2.One, 0.15);
		_tween.TweenProperty(this, "position:y", _originalPosition.Y, 0.15);
		_tween.TweenProperty(this, "rotation_degrees", _originalRotation, 0.15);
		ZIndex = 0;
	}

	public override void _GuiInput(InputEvent @event)
	{
		if (@event is InputEventMouseButton { Pressed: true, ButtonIndex: MouseButton.Left })
		{
			GD.Print("Hello world");
		}
	}
}
