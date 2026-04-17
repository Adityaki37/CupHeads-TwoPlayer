Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports System.Text
Imports UnityEngine
Imports UnityEngine.U2D
Imports UnityEngine.UI

' Token: 0x02000452 RID: 1106
Public Class AchievementsGUI
	Inherits AbstractMonoBehaviour

	' Token: 0x1700029E RID: 670
	' (get) Token: 0x060010A1 RID: 4257 RVA: 0x0009FA03 File Offset: 0x0009DE03
	' (set) Token: 0x060010A2 RID: 4258 RVA: 0x0009FA0B File Offset: 0x0009DE0B
	Public Property achievementsMenuOpen As Boolean

	' Token: 0x1700029F RID: 671
	' (get) Token: 0x060010A3 RID: 4259 RVA: 0x0009FA14 File Offset: 0x0009DE14
	' (set) Token: 0x060010A4 RID: 4260 RVA: 0x0009FA1C File Offset: 0x0009DE1C
	Public Property inputEnabled As Boolean

	' Token: 0x170002A0 RID: 672
	' (get) Token: 0x060010A5 RID: 4261 RVA: 0x0009FA25 File Offset: 0x0009DE25
	' (set) Token: 0x060010A6 RID: 4262 RVA: 0x0009FA2D File Offset: 0x0009DE2D
	Public Property justClosed As Boolean

	' Token: 0x060010A7 RID: 4263 RVA: 0x0009FA38 File Offset: 0x0009DE38
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.defaultAtlas = AssetLoader(Of SpriteAtlas).GetCachedAsset("Achievements")
		Me.stringBuilder = New StringBuilder()
		Me.background.sprite = Me.defaultAtlas.GetSprite("cheev_bg")
		Me.unearnedBackground.sprite = Me.defaultAtlas.GetSprite("cheev_card_unearned")
		Me.achievementsMenuOpen = False
		Me.canvasGroup = MyBase.GetComponent(Of CanvasGroup)()
		Me.canvasGroup.alpha = 0F
	End Sub

	' Token: 0x060010A8 RID: 4264 RVA: 0x0009FABF File Offset: 0x0009DEBF
	Public Sub Init(checkIfDead As Boolean)
		Me.input = New CupheadInput.AnyPlayerInput(checkIfDead)
	End Sub

	' Token: 0x060010A9 RID: 4265 RVA: 0x0009FAD0 File Offset: 0x0009DED0
	Private Sub Update()
		Me.justClosed = False
		Me.timeSinceStart += Time.deltaTime
		If Me.timeSinceStart < 0.25F Then
			Return
		End If
		If Me.activeNavigationButton <> CupheadButton.None AndAlso Me.input.GetButtonUp(Me.activeNavigationButton) Then
			Me.activeNavigationButton = CupheadButton.None
		End If
		If Not Me.inputEnabled Then
			Return
		End If
		If Me.GetButtonDown(CupheadButton.Cancel) Then
			AudioManager.Play("level_menu_select")
			Me.HideAchievements()
		End If
		If Me.activeNavigationButton = CupheadButton.None Then
			If Me.GetButtonDown(CupheadButton.MenuUp) Then
				If Me.achievementIndex.y = 0 Then
					Me.rowOffset = Me.currentGridSize.y - AchievementsGUI.VisualGridSize.y
					Me.cursorIndex.y = AchievementsGUI.VisualGridSize.y - 1
					Me.achievementIndex.y = Me.currentGridSize.y - 1
				Else
					If Me.cursorIndex.y = 0 Then
						Me.rowOffset -= 1
					End If
					Me.cursorIndex.y = Mathf.Max(Me.cursorIndex.y - 1, 0)
					Me.achievementIndex.y = Mathf.Max(Me.achievementIndex.y - 1, 0)
				End If
				Me.refreshIcons()
				Me.updateSelection()
			ElseIf Me.GetButtonDown(CupheadButton.MenuDown) Then
				If Me.achievementIndex.y = Me.currentGridSize.y - 1 Then
					Me.rowOffset = 0
					Me.cursorIndex.y = 0
					Me.achievementIndex.y = 0
				Else
					If Me.cursorIndex.y = AchievementsGUI.VisualGridSize.y - 1 Then
						Me.rowOffset = Mathf.Min(Me.rowOffset + 1, Me.currentGridSize.y - AchievementsGUI.VisualGridSize.y)
					End If
					Me.cursorIndex.y = Mathf.Min(Me.cursorIndex.y + 1, AchievementsGUI.VisualGridSize.y - 1)
					Me.achievementIndex.y = Mathf.Min(Me.achievementIndex.y + 1, Me.currentGridSize.y - 1)
				End If
				Me.refreshIcons()
				Me.updateSelection()
			ElseIf Me.GetButtonDown(CupheadButton.MenuLeft) Then
				Me.cursorIndex.x = Me.cursorIndex.x - 1
				If Me.cursorIndex.x < 0 Then
					Me.cursorIndex.x = AchievementsGUI.VisualGridSize.x - 1
				End If
				Me.achievementIndex.x = Me.achievementIndex.x - 1
				If Me.achievementIndex.x < 0 Then
					Me.achievementIndex.x = Me.currentGridSize.x - 1
				End If
				Me.updateSelection()
			ElseIf Me.GetButtonDown(CupheadButton.MenuRight) Then
				Me.cursorIndex.x = Me.cursorIndex.x + 1
				If Me.cursorIndex.x >= AchievementsGUI.VisualGridSize.x Then
					Me.cursorIndex.x = 0
				End If
				Me.achievementIndex.x = Me.achievementIndex.x + 1
				If Me.achievementIndex.x >= Me.currentGridSize.x Then
					Me.achievementIndex.x = 0
				End If
				Me.updateSelection()
			End If
		End If
	End Sub

	' Token: 0x060010AA RID: 4266 RVA: 0x0009FE70 File Offset: 0x0009E270
	Public Sub ShowAchievements()
		Me.handleDLCStatus()
		Me.cursorIndex = Vector2Int.zero
		Me.achievementIndex = Vector2Int.zero
		Me.rowOffset = 0
		Me.refreshIcons()
		Me.updateSelection()
		If Me.dlcEnabled Then
			Me.arrowCoroutine = MyBase.StartCoroutine(Me.arrow_cr())
		End If
		Me.timeSinceStart = 0F
		Me.achievementsMenuOpen = True
		Me.canvasGroup.alpha = 1F
		MyBase.FrameDelayedCallback(AddressOf Me.interactable, 1)
	End Sub

	' Token: 0x060010AB RID: 4267 RVA: 0x0009FF00 File Offset: 0x0009E300
	Public Sub HideAchievements()
		If Me.dlcEnabled Then
			MyBase.StopCoroutine(Me.arrowCoroutine)
			Me.arrowCoroutine = Nothing
		End If
		Me.canvasGroup.alpha = 0F
		Me.canvasGroup.interactable = False
		Me.canvasGroup.blocksRaycasts = False
		Me.inputEnabled = False
		Me.achievementsMenuOpen = False
		Me.justClosed = True
	End Sub

	' Token: 0x060010AC RID: 4268 RVA: 0x0009FF68 File Offset: 0x0009E368
	Private Sub interactable()
		Me.canvasGroup.interactable = True
		Me.canvasGroup.blocksRaycasts = True
		Me.inputEnabled = True
	End Sub

	' Token: 0x060010AD RID: 4269 RVA: 0x0009FF8C File Offset: 0x0009E38C
	Private Sub updateSelection()
		Dim achievementIcon As AchievementIcon = Me.iconRows(Me.cursorIndex.y).achievementIcons(Me.cursorIndex.x)
		Me.cursor.position = achievementIcon.transform.position
		Dim num As Integer = Me.achievementIndex.y * Me.currentGridSize.x + Me.achievementIndex.x
		Dim achievement As LocalAchievementsManager.Achievement = CType(num, LocalAchievementsManager.Achievement)
		Dim text As String = achievement.ToString()
		Dim flag As Boolean = LocalAchievementsManager.IsAchievementUnlocked(achievement)
		Dim flag2 As Boolean = LocalAchievementsManager.IsHiddenAchievement(achievement)
		If flag OrElse Not flag2 Then
			Dim text2 As String = "Achievement" + text + "Title"
			Me.titleLocalization.ApplyTranslation(Localization.Find(text2), Nothing)
			Dim text3 As String = "Achievement" + text + "Desc"
			Me.descriptionLocalization.ApplyTranslation(Localization.Find(text3), Nothing)
		Else
			Me.titleText.text = AchievementsGUI.TitleHidden
			Me.titleText.font = FontLoader.GetFont(AchievementsGUI.TitleHiddenFont)
			Me.descriptionText.text = AchievementsGUI.DescriptionHidden
			Me.descriptionText.font = FontLoader.GetFont(AchievementsGUI.DescriptionHiddenFont)
		End If
		Dim text4 As String = text
		If flag Then
			text4 += "_earned"
		End If
		Dim achievementSprite As Sprite = Me.getAchievementSprite(text4, achievement)
		Me.largeIcon.sprite = achievementSprite
		Me.titleText.color = If((Not flag), AchievementsGUI.LockedTextColor, AchievementsGUI.UnlockedTextColor)
		Me.descriptionText.color = If((Not flag), AchievementsGUI.LockedTextColor, AchievementsGUI.UnlockedTextColor)
		Me.unearnedBackground.enabled = Not flag
		Me.noise.sprite = Me.getSprite(If((Not flag), "cheev_card_noise_unearned", "cheev_card_noise_earned"), Me.defaultAtlas)
		AudioManager.Play("level_menu_move")
	End Sub

	' Token: 0x060010AE RID: 4270 RVA: 0x000A0177 File Offset: 0x0009E577
	Protected Function GetButtonDown(button As CupheadButton) As Boolean
		If Me.input.GetButtonDown(button) Then
			Me.activeNavigationButton = button
			Return True
		End If
		Return False
	End Function

	' Token: 0x060010AF RID: 4271 RVA: 0x000A0194 File Offset: 0x0009E594
	Private Sub handleDLCStatus()
		Me.dlcEnabled = DLCManager.DLCEnabled()
		Me.currentGridSize = If((Not Me.dlcEnabled), AchievementsGUI.VisualGridSize, AchievementsGUI.GridSize)
		If Me.dlcEnabled AndAlso Me.dlcAtlas Is Nothing Then
			Me.dlcAtlas = AssetLoader(Of SpriteAtlas).GetCachedAsset("Achievements_DLC")
		End If
	End Sub

	' Token: 0x060010B0 RID: 4272 RVA: 0x000A01F8 File Offset: 0x0009E5F8
	Private Iterator Function arrow_cr() As IEnumerator
		Dim index As Integer = 0
		Dim wait As WaitForFrameTimePersistent = New WaitForFrameTimePersistent(0.083333336F, True)
		While True
			Me.topArrow.sprite = Me.arrowSprites(index)
			index = MathUtilities.NextIndex(index, Me.arrowSprites.Length)
			Me.bottomArrow.sprite = Me.arrowSprites(MathUtilities.NextIndex(index, Me.arrowSprites.Length))
			Yield wait
		End While
		Return
	End Function

	' Token: 0x060010B1 RID: 4273 RVA: 0x000A0214 File Offset: 0x0009E614
	Private Sub refreshIcons()
		If Me.dlcEnabled Then
			Me.topArrow.enabled = Me.rowOffset <> 0
			Me.bottomArrow.enabled = Me.rowOffset <> 2
		Else
			Dim behaviour As Behaviour = Me.topArrow
			Dim flag As Boolean = False
			Me.bottomArrow.enabled = flag
			behaviour.enabled = flag
		End If
		Dim num As Integer = Me.rowOffset * Me.currentGridSize.x
		For Each iconRow As AchievementsGUI.IconRow In Me.iconRows
			For Each achievementIcon As AchievementIcon In iconRow.achievementIcons
				Me.stringBuilder.Length = 0
				Dim achievement As LocalAchievementsManager.Achievement = CType(num, LocalAchievementsManager.Achievement)
				Me.stringBuilder.Append(AchievementsGUI.AchievementNames(num))
				If LocalAchievementsManager.IsAchievementUnlocked(achievement) Then
					Me.stringBuilder.Append("_earned")
				End If
				Me.stringBuilder.Append("_sm")
				Dim achievementSprite As Sprite = Me.getAchievementSprite(Me.stringBuilder.ToString(), achievement)
				achievementIcon.SetIcon(achievementSprite)
				num += 1
			Next
		Next
	End Sub

	' Token: 0x060010B2 RID: 4274 RVA: 0x000A034C File Offset: 0x0009E74C
	Private Function getSprite(spriteName As String, atlas As SpriteAtlas) As Sprite
		Dim sprite As Sprite
		If Not Me.spriteCache.TryGetValue(spriteName, sprite) Then
			sprite = atlas.GetSprite(spriteName)
			Me.spriteCache.Add(spriteName, sprite)
		End If
		Return sprite
	End Function

	' Token: 0x060010B3 RID: 4275 RVA: 0x000A0384 File Offset: 0x0009E784
	Private Function getAchievementSprite(spriteName As String, achievement As LocalAchievementsManager.Achievement) As Sprite
		Dim spriteAtlas As SpriteAtlas
		If Array.IndexOf(Of LocalAchievementsManager.Achievement)(LocalAchievementsManager.DLCAchievements, achievement) >= 0 Then
			spriteAtlas = Me.dlcAtlas
		Else
			spriteAtlas = Me.defaultAtlas
		End If
		Return Me.getSprite(spriteName, spriteAtlas)
	End Function

	' Token: 0x040019D4 RID: 6612
	Private Shared AchievementNames As String() = [Enum].GetNames(GetType(LocalAchievementsManager.Achievement))

	' Token: 0x040019D5 RID: 6613
	Private Shared UnlockedTextColor As Color = New Color(0.9098039F, 0.8235294F, 0.68235296F)

	' Token: 0x040019D6 RID: 6614
	Private Shared LockedTextColor As Color = New Color(0.27058825F, 0.26666668F, 0.2627451F)

	' Token: 0x040019D7 RID: 6615
	Private Shared TitleHidden As String = "? ? ? ? ? ? ?"

	' Token: 0x040019D8 RID: 6616
	Private Shared TitleHiddenFont As FontLoader.FontType = FontLoader.FontType.CupheadMemphis_Medium_merged

	' Token: 0x040019D9 RID: 6617
	Private Shared DescriptionHidden As String = "?  ?  ?  ?  ?  ?"

	' Token: 0x040019DA RID: 6618
	Private Shared DescriptionHiddenFont As FontLoader.FontType = FontLoader.FontType.CupheadVogue_Bold_merged

	' Token: 0x040019DB RID: 6619
	Private Shared GridSize As Vector2Int = New Vector2Int(7, 6)

	' Token: 0x040019DC RID: 6620
	Private Shared VisualGridSize As Vector2Int = New Vector2Int(7, 4)

	' Token: 0x040019DD RID: 6621
	<SerializeField()>
	Private iconRows As AchievementsGUI.IconRow()

	' Token: 0x040019DE RID: 6622
	<SerializeField()>
	Private cursor As RectTransform

	' Token: 0x040019DF RID: 6623
	<SerializeField()>
	Private topArrow As Image

	' Token: 0x040019E0 RID: 6624
	<SerializeField()>
	Private bottomArrow As Image

	' Token: 0x040019E1 RID: 6625
	<SerializeField()>
	Private background As Image

	' Token: 0x040019E2 RID: 6626
	<SerializeField()>
	Private unearnedBackground As Image

	' Token: 0x040019E3 RID: 6627
	<SerializeField()>
	Private titleText As Text

	' Token: 0x040019E4 RID: 6628
	<SerializeField()>
	Private descriptionText As Text

	' Token: 0x040019E5 RID: 6629
	<SerializeField()>
	Private titleLocalization As LocalizationHelper

	' Token: 0x040019E6 RID: 6630
	<SerializeField()>
	Private descriptionLocalization As LocalizationHelper

	' Token: 0x040019E7 RID: 6631
	<SerializeField()>
	Private largeIcon As Image

	' Token: 0x040019E8 RID: 6632
	<SerializeField()>
	Private noise As Image

	' Token: 0x040019E9 RID: 6633
	<SerializeField()>
	Private arrowSprites As Sprite()

	' Token: 0x040019EA RID: 6634
	Private input As CupheadInput.AnyPlayerInput

	' Token: 0x040019EB RID: 6635
	Private timeSinceStart As Single

	' Token: 0x040019EC RID: 6636
	Private achievementIndex As Vector2Int

	' Token: 0x040019ED RID: 6637
	Private cursorIndex As Vector2Int

	' Token: 0x040019EE RID: 6638
	Private rowOffset As Integer

	' Token: 0x040019EF RID: 6639
	Private defaultAtlas As SpriteAtlas

	' Token: 0x040019F0 RID: 6640
	Private dlcAtlas As SpriteAtlas

	' Token: 0x040019F1 RID: 6641
	Private spriteCache As Dictionary(Of String, Sprite) = New Dictionary(Of String, Sprite)()

	' Token: 0x040019F2 RID: 6642
	Private activeNavigationButton As CupheadButton = CupheadButton.None

	' Token: 0x040019F3 RID: 6643
	Private stringBuilder As StringBuilder

	' Token: 0x040019F4 RID: 6644
	Private arrowCoroutine As Coroutine

	' Token: 0x040019F5 RID: 6645
	Private dlcEnabled As Boolean

	' Token: 0x040019F6 RID: 6646
	Private currentGridSize As Vector2Int

	' Token: 0x040019F7 RID: 6647
	Private canvasGroup As CanvasGroup

	' Token: 0x02000453 RID: 1107
	<Serializable()>
	Public Class IconRow
		' Token: 0x040019FB RID: 6651
		Public achievementIcons As AchievementIcon()
	End Class
End Class
