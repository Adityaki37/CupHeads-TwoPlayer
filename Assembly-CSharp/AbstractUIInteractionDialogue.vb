Imports System
Imports TMPro
Imports UnityEngine
Imports UnityEngine.UI

' Token: 0x0200044E RID: 1102
Public MustInherit Class AbstractUIInteractionDialogue
	Inherits AbstractMonoBehaviour

	' Token: 0x17000299 RID: 665
	' (get) Token: 0x0600108E RID: 4238 RVA: 0x0009F5E4 File Offset: 0x0009D9E4
	Protected Overridable ReadOnly Property OpenTime As Single
		Get
			Return 0.3F
		End Get
	End Property

	' Token: 0x1700029A RID: 666
	' (get) Token: 0x0600108F RID: 4239 RVA: 0x0009F5EB File Offset: 0x0009D9EB
	Protected Overridable ReadOnly Property CloseTime As Single
		Get
			Return 0.3F
		End Get
	End Property

	' Token: 0x1700029B RID: 667
	' (get) Token: 0x06001090 RID: 4240 RVA: 0x0009F5F2 File Offset: 0x0009D9F2
	Protected Overridable ReadOnly Property OpenScale As Single
		Get
			Return 1F
		End Get
	End Property

	' Token: 0x1700029C RID: 668
	' (get) Token: 0x06001091 RID: 4241 RVA: 0x0009F5F9 File Offset: 0x0009D9F9
	' (set) Token: 0x06001092 RID: 4242 RVA: 0x0009F606 File Offset: 0x0009DA06
	Protected Property Text As String
		Get
			Return Me.tmpText.text
		End Get
		Set(value As String)
			Me.tmpText.text = value
		End Set
	End Property

	' Token: 0x1700029D RID: 669
	' (get) Token: 0x06001093 RID: 4243 RVA: 0x0009F614 File Offset: 0x0009DA14
	Protected Overridable ReadOnly Property PreferredWidth As Single
		Get
			Return Me.tmpText.preferredWidth + Me.glyph.preferredWidth
		End Get
	End Property

	' Token: 0x06001094 RID: 4244 RVA: 0x0009F62D File Offset: 0x0009DA2D
	Private Sub Start()
		MyBase.transform.localScale = Vector3.zero
	End Sub

	' Token: 0x06001095 RID: 4245 RVA: 0x0009F640 File Offset: 0x0009DA40
	Protected Overridable Sub Init(properties As AbstractUIInteractionDialogue.Properties, player As PlayerInput, offset As Vector2)
		Dim num As Single = 40F
		Me.target = player.transform
		Me.dialogueOffset = offset
		Dim num2 As Integer
		If Parser.IntTryParse(properties.text, num2) Then
			Me.Text = Localization.Translate(num2).text.ToUpper()
			Me.tmpText.font = Localization.Instance.fonts(CInt(Localization.language))(28).fontAsset
		Else
			Me.Text = properties.text.ToUpper()
		End If
		Me.glyph.rewiredPlayerId = CInt(player.playerId)
		Me.glyph.button = properties.button
		Me.glyph.Init()
		Me.back.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Me.PreferredWidth + 10F)
		Me.back.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, num + 11F)
		MyBase.TweenValue(0F, 1F, Me.OpenTime, EaseUtils.EaseType.linear, AddressOf Me.OpenTween)
	End Sub

	' Token: 0x06001096 RID: 4246 RVA: 0x0009F750 File Offset: 0x0009DB50
	Public Sub Close()
		Me.closeScale = MyBase.transform.localScale.x
		Me.StopAllCoroutines()
		MyBase.TweenValue(0F, 1F, Me.CloseTime, EaseUtils.EaseType.linear, AddressOf Me.CloseTween)
	End Sub

	' Token: 0x06001097 RID: 4247 RVA: 0x0009F7A4 File Offset: 0x0009DBA4
	Protected Overridable Sub OpenTween(value As Single)
		Dim num As Single = 40F
		Me.back.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Me.PreferredWidth + 10F)
		Me.back.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, num + 11F)
		MyBase.transform.localScale = Vector3.one * EaseUtils.Ease(EaseUtils.EaseType.easeOutSine, 0F, Me.OpenScale, value)
	End Sub

	' Token: 0x06001098 RID: 4248 RVA: 0x0009F80C File Offset: 0x0009DC0C
	Protected Overridable Sub CloseTween(value As Single)
		MyBase.transform.localScale = Vector3.one * EaseUtils.Ease(EaseUtils.EaseType.easeInBack, Me.closeScale, 0F, value)
		If MyBase.transform.localScale.x < 0.001F Then
			Me.StopAllCoroutines()
			Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
		End If
	End Sub

	' Token: 0x040019C1 RID: 6593
	Protected Const PADDINGH As Single = 10F

	' Token: 0x040019C2 RID: 6594
	Protected Const PADDINGV As Single = 11F

	' Token: 0x040019C3 RID: 6595
	<SerializeField()>
	Protected uiText As Text

	' Token: 0x040019C4 RID: 6596
	<SerializeField()>
	Protected tmpText As TextMeshProUGUI

	' Token: 0x040019C5 RID: 6597
	<SerializeField()>
	Protected glyph As CupheadGlyph

	' Token: 0x040019C6 RID: 6598
	<SerializeField()>
	Protected back As RectTransform

	' Token: 0x040019C7 RID: 6599
	Protected target As Transform

	' Token: 0x040019C8 RID: 6600
	Protected dialogueOffset As Vector2

	' Token: 0x040019C9 RID: 6601
	Private closeScale As Single

	' Token: 0x0200044F RID: 1103
	Public Enum AnimationType
		' Token: 0x040019CB RID: 6603
		Full
		' Token: 0x040019CC RID: 6604
		Individual
	End Enum

	' Token: 0x02000450 RID: 1104
	<Serializable()>
	Public Class Properties
		' Token: 0x06001099 RID: 4249 RVA: 0x0009F870 File Offset: 0x0009DC70
		Public Sub New()
			Me.text = String.Empty
			Me.subtext = String.Empty
			Me.button = CupheadButton.Accept
			Me.animationType = AbstractUIInteractionDialogue.AnimationType.Full
		End Sub

		' Token: 0x0600109A RID: 4250 RVA: 0x0009F8C8 File Offset: 0x0009DCC8
		Public Sub New(text As String)
			Me.text = text
			Me.subtext = String.Empty
			Me.button = CupheadButton.Accept
			Me.animationType = AbstractUIInteractionDialogue.AnimationType.Full
		End Sub

		' Token: 0x0600109B RID: 4251 RVA: 0x0009F91C File Offset: 0x0009DD1C
		Public Sub New(text As String, button As CupheadButton)
			Me.text = text
			Me.subtext = String.Empty
			Me.button = button
			Me.animationType = AbstractUIInteractionDialogue.AnimationType.Full
		End Sub

		' Token: 0x0600109C RID: 4252 RVA: 0x0009F970 File Offset: 0x0009DD70
		Public Sub New(text As String, button As CupheadButton, animationType As AbstractUIInteractionDialogue.AnimationType)
			Me.text = text
			Me.subtext = String.Empty
			Me.button = button
			Me.animationType = animationType
		End Sub

		' Token: 0x040019CD RID: 6605
		Public Const DEFAULT_ANIM_TYPE As AbstractUIInteractionDialogue.AnimationType = AbstractUIInteractionDialogue.AnimationType.Full

		' Token: 0x040019CE RID: 6606
		Public Const DEFAULT_BUTTON As CupheadButton = CupheadButton.Accept

		' Token: 0x040019CF RID: 6607
		Public text As String = String.Empty

		' Token: 0x040019D0 RID: 6608
		Public subtext As String = String.Empty

		' Token: 0x040019D1 RID: 6609
		Public animationType As AbstractUIInteractionDialogue.AnimationType

		' Token: 0x040019D2 RID: 6610
		Public button As CupheadButton = CupheadButton.Accept
	End Class
End Class
