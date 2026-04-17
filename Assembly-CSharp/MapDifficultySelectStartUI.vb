Imports System
Imports TMPro
Imports UnityEngine
Imports UnityEngine.UI

' Token: 0x0200099B RID: 2459
Public Class MapDifficultySelectStartUI
	Inherits AbstractMapSceneStartUI

	' Token: 0x170004AB RID: 1195
	' (get) Token: 0x06003988 RID: 14728 RVA: 0x0020A3C9 File Offset: 0x002087C9
	' (set) Token: 0x06003989 RID: 14729 RVA: 0x0020A3D0 File Offset: 0x002087D0
	Public Shared Property Current As MapDifficultySelectStartUI

	' Token: 0x170004AC RID: 1196
	' (get) Token: 0x0600398A RID: 14730 RVA: 0x0020A3D8 File Offset: 0x002087D8
	' (set) Token: 0x0600398B RID: 14731 RVA: 0x0020A3DF File Offset: 0x002087DF
	Public Shared Property Mode As Level.Mode

	' Token: 0x0600398C RID: 14732 RVA: 0x0020A3E8 File Offset: 0x002087E8
	Protected Overrides Sub Awake()
		MyBase.Awake()
		MapDifficultySelectStartUI.Current = Me
		Select Case Level.CurrentMode
			Case Level.Mode.Easy
				Me.index = 0
			Case Level.Mode.Normal
				Me.index = 1
			Case Level.Mode.Hard
				Me.index = 2
		End Select
		Me.options = New Level.Mode() { Level.Mode.Easy, Level.Mode.Normal, Level.Mode.Hard }
		Me.SetDifficultyAvailability()
		Me.difficulyTexts = New TMP_Text(2) {}
		Me.difficulyTexts(0) = Me.easy.GetComponent(Of TMP_Text)()
		Me.difficulyTexts(1) = Me.normal.GetComponent(Of TMP_Text)()
		Me.difficulyTexts(2) = Me.hard.GetComponent(Of TMP_Text)()
		If Me.bossImage IsNot Nothing AndAlso Me.bossImage.textComponent IsNot Nothing Then
			Me.initialMaxFontSize = Me.bossImage.textComponent.resizeTextMaxSize
		End If
		Me.initialinImagePosX = Me.inAnimated.rectTransform.offsetMin
		Me.initialinImagePosY = Me.inAnimated.rectTransform.offsetMax
		Me.initialinDifficultyPos = Me.difficultyImage.rectTransform.anchoredPosition
		Me.initialDifficultyPos = Me.difficultySelectionText.rectTransform.anchoredPosition
		Me.initialBossNamePos = Me.bossNameImage.rectTransform.anchoredPosition
	End Sub

	' Token: 0x0600398D RID: 14733 RVA: 0x0020A554 File Offset: 0x00208954
	Private Sub SetDifficultyAvailability()
		If PlayerData.Data.CurrentMap = Scenes.scene_map_world_4 Then
			If Not PlayerData.Data.IsHardModeAvailable Then
				Me.options = New Level.Mode() { Level.Mode.Normal }
				Me.hard.gameObject.SetActive(False)
				Me.hardSeparator.gameObject.SetActive(False)
			Else
				Me.options = New Level.Mode() { Level.Mode.Normal, Level.Mode.Hard }
			End If
			Me.index = Mathf.Max(0, Me.index - 1)
			Me.easy.gameObject.SetActive(False)
			Me.normalSeparator.gameObject.SetActive(False)
		ElseIf PlayerData.Data.CurrentMap = Scenes.scene_map_world_DLC Then
			If Me.level = "Saltbaker" Then
				If Not PlayerData.Data.IsHardModeAvailableDLC Then
					Me.options = New Level.Mode() { Level.Mode.Normal }
				Else
					Me.options = New Level.Mode() { Level.Mode.Normal, Level.Mode.Hard }
				End If
			ElseIf Not PlayerData.Data.IsHardModeAvailableDLC Then
				Me.options = New Level.Mode() { Level.Mode.Easy, Level.Mode.Normal }
			Else
				Me.options = New Level.Mode() { Level.Mode.Easy, Level.Mode.Normal, Level.Mode.Hard }
			End If
			Me.easy.gameObject.SetActive(Me.level <> "Saltbaker")
			Me.normalSeparator.gameObject.SetActive(Me.level <> "Saltbaker")
			Me.hard.gameObject.SetActive(PlayerData.Data.IsHardModeAvailableDLC)
			Me.hardSeparator.gameObject.SetActive(PlayerData.Data.IsHardModeAvailableDLC)
		Else
			If Not PlayerData.Data.IsHardModeAvailable Then
				Me.options = New Level.Mode() { Level.Mode.Easy, Level.Mode.Normal }
			End If
			Me.hard.gameObject.SetActive(PlayerData.Data.IsHardModeAvailable)
			Me.hardSeparator.gameObject.SetActive(PlayerData.Data.IsHardModeAvailable)
		End If
	End Sub

	' Token: 0x0600398E RID: 14734 RVA: 0x0020A770 File Offset: 0x00208B70
	Public Sub [In](playerController As MapPlayerController)
		MyBase.[In](playerController)
		If Level.CurrentMode = Level.Mode.Easy AndAlso PlayerData.Data.CurrentMap = Scenes.scene_map_world_4 Then
			Level.SetCurrentMode(Level.Mode.Normal)
			Select Case Level.CurrentMode
				Case Level.Mode.Easy
					Me.index = 0
				Case Level.Mode.Normal
					Me.index = 1
				Case Level.Mode.Hard
					Me.index = 2
			End Select
		End If
		If PlayerData.Data.CurrentMap = Scenes.scene_map_world_DLC Then
			Me.SetDifficultyAvailability()
			If Me.level = "Saltbaker" AndAlso Level.CurrentMode = Level.Mode.Easy Then
				Level.SetCurrentMode(Level.Mode.Normal)
			End If
		End If
		If MyBase.animator IsNot Nothing Then
			MyBase.animator.SetTrigger("ZoomIn")
			AudioManager.Play("world_map_level_menu_open")
		End If
		Me.InWordSetup()
		Me.difficultyImage.enabled = Localization.language = Localization.Languages.Japanese
		Me.difficultyImage.rectTransform.anchoredPosition = Me.initialinDifficultyPos
		For i As Integer = 0 To Me.separatorsAnimated.Length - 1
			Me.separatorsAnimated(i).sprite = Me.separatorsSprites(Global.UnityEngine.Random.Range(0, Me.separatorsSprites.Length))
		Next
		Dim flag As Boolean = Localization.language = Localization.Languages.Korean OrElse Localization.language = Localization.Languages.SimplifiedChinese OrElse Localization.language = Localization.Languages.Japanese
		Me.bossTitleImage.enabled = Localization.language = Localization.Languages.English OrElse flag OrElse PlayerData.Data.CurrentMap = Scenes.scene_map_world_DLC
		Me.glowScript.StopGlow()
		Me.glowScript.DisableTMPText()
		Me.glowScript.DisableImages()
		If Localization.language = Localization.Languages.SimplifiedChinese Then
			Me.difficultySelectionText.rectTransform.anchoredPosition = New Vector2(Me.difficultySelectionText.rectTransform.anchoredPosition.x, -70F)
		Else
			Me.difficultySelectionText.rectTransform.anchoredPosition = Me.initialDifficultyPos
		End If
		Dim translationElement As TranslationElement = Localization.Find(Me.level + "Selection")
		If Me.bossImage IsNot Nothing AndAlso translationElement IsNot Nothing Then
			Me.bossImage.ApplyTranslation(translationElement, Nothing)
			If Me.bossImage.textComponent IsNot Nothing Then
				If Localization.language = Localization.Languages.Korean Then
					Me.bossImage.textComponent.resizeTextMaxSize = 100
				Else
					Me.bossImage.textComponent.resizeTextMaxSize = Me.initialMaxFontSize
				End If
			End If
			If flag Then
				Me.SetupAsianBossCard(translationElement, Me.bossTitleImage)
			Else
				Me.bossImage.transform.localScale = Vector3.one
				Me.bossImage.transform.localPosition = Vector3.zero
				Me.bossTitleImage.rectTransform.offsetMax = New Vector2(Me.bossTitleImage.rectTransform.offsetMax.x, 0.5F)
				Me.bossTitleImage.rectTransform.offsetMin = New Vector2(Me.bossTitleImage.rectTransform.offsetMin.x, 0.5F)
				Me.inAnimated.rectTransform.offsetMin = Me.initialinImagePosX
				Me.inAnimated.rectTransform.offsetMax = Me.initialinImagePosY
				Me.inText.fontStyle = FontStyle.Italic
			End If
		End If
		Dim translationElement2 As TranslationElement = Localization.Find(Me.level + "WorldMap")
		If translationElement2 IsNot Nothing Then
			Me.bossName.ApplyTranslation(translationElement2, Nothing)
			If Me.bossName.textComponent IsNot Nothing AndAlso Me.bossName.textComponent.enabled Then
				Me.bossName.textComponent.font = FontLoader.GetFont(FontLoader.FontType.CupheadHenriette_A_merged)
			End If
			Me.bossNameImage.transform.localScale = Vector3.one
			Me.bossNameImage.rectTransform.anchoredPosition = Me.initialBossNamePos
			If flag Then
				Me.bossNameImage.material = Me.bossCardWhiteMaterial
				If Localization.language = Localization.Languages.Korean OrElse Localization.language = Localization.Languages.Japanese Then
					Me.bossNameImage.transform.localScale = New Vector3(1.2F, 1.2F, 1F)
					If Localization.language = Localization.Languages.Japanese Then
						Me.bossNameImage.rectTransform.anchoredPosition = New Vector2(0F, 214.2F)
					End If
				End If
			End If
			Me.bossName.gameObject.SetActive(Localization.language <> Localization.Languages.English AndAlso Not flag AndAlso PlayerData.Data.CurrentMap <> Scenes.scene_map_world_DLC)
		End If
		Dim translationElement3 As TranslationElement = Localization.Find(Me.level + "Glow")
		If Localization.language <> Localization.Languages.English Then
			If translationElement3 IsNot Nothing AndAlso flag Then
				Me.bossGlow.ApplyTranslation(translationElement3, Nothing)
			Else
				Me.glowScript.InitTMPText(New MaskableGraphic() { Me.bossImage.textMeshProComponent, Me.bossName.textComponent })
				Me.glowScript.BeginGlow()
			End If
		End If
		Me.bossGlow.gameObject.SetActive(flag AndAlso PlayerData.Data.CurrentMap <> Scenes.scene_map_world_DLC)
		For j As Integer = 0 To Me.difficulyTexts.Length - 1
			Me.difficulyTexts(j).color = Me.unselectedColor
		Next
		Me.difficulyTexts(CInt(Level.CurrentMode)).color = Me.selectedColor
	End Sub

	' Token: 0x0600398F RID: 14735 RVA: 0x0020AD26 File Offset: 0x00209126
	Private Sub OnDestroy()
		Me.bossNameImage.sprite = Nothing
		Me.bossTitleImage.sprite = Nothing
		Me.asianGlow.sprite = Nothing
		If MapDifficultySelectStartUI.Current Is Me Then
			MapDifficultySelectStartUI.Current = Nothing
		End If
	End Sub

	' Token: 0x06003990 RID: 14736 RVA: 0x0020AD62 File Offset: 0x00209162
	Private Sub Update()
		Me.UpdateCursor()
		If MyBase.CurrentState = AbstractMapSceneStartUI.State.Active Then
			Me.CheckInput()
		End If
	End Sub

	' Token: 0x06003991 RID: 14737 RVA: 0x0020AD7C File Offset: 0x0020917C
	Private Sub CheckInput()
		If Not MyBase.Able Then
			Return
		End If
		If MyBase.GetButtonDown(CupheadButton.MenuLeft) Then
			Me.[Next](-1)
		End If
		If MyBase.GetButtonDown(CupheadButton.MenuRight) Then
			Me.[Next](1)
		End If
		If MyBase.GetButtonDown(CupheadButton.Cancel) Then
			MyBase.Out()
		End If
		If MyBase.GetButtonDown(CupheadButton.Accept) Then
			MyBase.LoadLevel()
		End If
	End Sub

	' Token: 0x06003992 RID: 14738 RVA: 0x0020ADE4 File Offset: 0x002091E4
	Private Sub [Next](direction As Integer)
		If(Me.index <> Me.options.Length - 1 AndAlso direction <> -1) OrElse (Me.index <> 0 AndAlso direction <> 1) Then
			AudioManager.Play("world_map_level_difficulty_hover")
		End If
		Me.index = Mathf.Clamp(Me.index + direction, 0, Me.options.Length - 1)
		Level.SetCurrentMode(Me.options(Me.index))
		Me.UpdateCursor()
		For i As Integer = 0 To Me.difficulyTexts.Length - 1
			Me.difficulyTexts(i).color = Me.unselectedColor
		Next
		Me.difficulyTexts(CInt(Level.CurrentMode)).color = Me.selectedColor
	End Sub

	' Token: 0x06003993 RID: 14739 RVA: 0x0020AEA4 File Offset: 0x002092A4
	Private Sub UpdateCursor()
		Dim position As Vector3 = Me.cursor.transform.position
		position.y = Me.normal.position.y
		Dim mode As Level.Mode = Level.CurrentMode
		If PlayerData.Data.CurrentMap = Scenes.scene_map_world_4 AndAlso mode = Level.Mode.Easy Then
			mode = Level.Mode.Normal
		End If
		Select Case mode
			Case Level.Mode.Easy
				position.x = Me.easy.position.x
				Me.cursor.sizeDelta = New Vector2(Me.easy.sizeDelta.x + 30F, Me.easy.sizeDelta.y + 20F)
			Case Level.Mode.Normal
				position.x = Me.normal.position.x
				Me.cursor.sizeDelta = New Vector2(Me.normal.sizeDelta.x + 30F, Me.normal.sizeDelta.y + 20F)
			Case Level.Mode.Hard
				position.x = Me.hard.position.x
				Me.cursor.sizeDelta = New Vector2(Me.hard.sizeDelta.x + 30F, Me.hard.sizeDelta.y + 20F)
		End Select
		Me.cursor.transform.position = position
	End Sub

	' Token: 0x06003994 RID: 14740 RVA: 0x0020B054 File Offset: 0x00209454
	Private Sub SetupAsianBossCard(translation As TranslationElement, image As Image)
		image.material = Me.bossCardWhiteMaterial
		image.rectTransform.offsetMax = New Vector2(image.rectTransform.offsetMax.x, 0F)
		image.rectTransform.offsetMin = New Vector2(image.rectTransform.offsetMin.x, 0F)
		Me.SetupAsianDifficulty()
		image.transform.localScale = New Vector3(0.9F, 0.9F, 1F)
		If Localization.language = Localization.Languages.Korean Then
			If PlayerData.Data.CurrentMap <> Scenes.scene_map_world_DLC Then
				image.rectTransform.offsetMax = New Vector2(image.rectTransform.offsetMax.x, 40F)
				image.rectTransform.offsetMin = New Vector2(image.rectTransform.offsetMin.x, 40F)
			End If
			Me.SetupKoreanInWord()
		ElseIf Localization.language = Localization.Languages.SimplifiedChinese Then
			Me.inAnimated.rectTransform.offsetMax = New Vector2(Me.inAnimated.rectTransform.offsetMax.x, -140F)
			If Me.level.Equals("FlyingBlimp") Then
				image.transform.localScale = New Vector3(0.8F, 0.8F, 1F)
				image.rectTransform.offsetMax = New Vector2(image.rectTransform.offsetMax.x, 40F)
				image.rectTransform.offsetMin = New Vector2(image.rectTransform.offsetMin.x, 40F)
				Me.inAnimated.rectTransform.offsetMax = New Vector2(Me.inAnimated.rectTransform.offsetMax.x, -100F)
			End If
		ElseIf Localization.language = Localization.Languages.Japanese Then
			If Me.level.Equals("Flower") OrElse Me.level.Equals("FlyingBird") OrElse Me.level.Equals("Mouse") OrElse Me.level.Equals("SallyStagePlay") Then
				image.transform.localScale = New Vector3(1.2F, 1.2F, 1F)
				image.rectTransform.offsetMax = New Vector2(image.rectTransform.offsetMax.x, -90F)
			ElseIf Me.level.Equals("Train") Then
				image.transform.localScale = New Vector3(0.8F, 0.8F, 1F)
				image.rectTransform.offsetMax = New Vector2(image.rectTransform.offsetMax.x, 70F)
			ElseIf Me.level.Equals("Bee") Then
				image.transform.localScale = Vector3.one
				image.rectTransform.offsetMax = New Vector2(image.rectTransform.offsetMax.x, -60F)
			ElseIf Me.level.Equals("DicePalaceMain") Then
				image.transform.localScale = New Vector3(1.1F, 1.1F, 1F)
				image.rectTransform.offsetMax = New Vector2(image.rectTransform.offsetMax.x, -70F)
			Else
				image.transform.localScale = New Vector3(0.9F, 0.9F, 1F)
				image.rectTransform.offsetMax = New Vector2(image.rectTransform.offsetMax.x, 55F)
			End If
			Me.difficultyImage.rectTransform.anchoredPosition = New Vector2(0F, -70F)
			Me.SetupJapaneseInWord()
		End If
	End Sub

	' Token: 0x06003995 RID: 14741 RVA: 0x0020B488 File Offset: 0x00209888
	Private Sub SetupAsianDifficulty()
		Me.difficultyText.textComponent.fontSize = 29
		Me.easy.gameObject.GetComponent(Of TMP_Text)().fontSize = 37F
		Me.normal.gameObject.GetComponent(Of TMP_Text)().fontSize = 37F
		Me.hard.gameObject.GetComponent(Of TMP_Text)().fontSize = 37F
	End Sub

	' Token: 0x06003996 RID: 14742 RVA: 0x0020B4F8 File Offset: 0x002098F8
	Private Sub SetupJapaneseInWord()
		If PlayerData.Data.CurrentMap = Scenes.scene_map_world_DLC Then
			Me.inAnimated.enabled = False
			Me.inText.enabled = False
			Return
		End If
		Me.inAnimated.preserveAspect = True
		If Me.level.Equals("Flower") OrElse Me.level.Equals("FlyingBird") OrElse Me.level.Equals("Mouse") OrElse Me.level.Equals("SallyStagePlay") OrElse Me.level.Equals("Bee") OrElse Me.level.Equals("DicePalaceMain") Then
			Me.inAnimated.rectTransform.offsetMax = New Vector2(Me.inAnimated.rectTransform.offsetMax.x, Me.initialinImagePosY.y - 15.9F)
			Me.inAnimated.rectTransform.offsetMin = New Vector2(Me.inAnimated.rectTransform.offsetMin.x, Me.initialinImagePosX.y - 15.9F)
		Else
			Me.inAnimated.rectTransform.offsetMax = New Vector2(Me.inAnimated.rectTransform.offsetMax.x, Me.initialinImagePosY.y + 6.5F)
			Me.inAnimated.rectTransform.offsetMin = New Vector2(Me.inAnimated.rectTransform.offsetMin.x, Me.initialinImagePosX.y + 6.5F)
		End If
	End Sub

	' Token: 0x06003997 RID: 14743 RVA: 0x0020B6B8 File Offset: 0x00209AB8
	Private Sub SetupKoreanInWord()
		If PlayerData.Data.CurrentMap = Scenes.scene_map_world_DLC Then
			Me.inAnimated.enabled = False
			Me.inText.enabled = False
			Return
		End If
		Me.inText.fontStyle = FontStyle.Normal
		If Me.level.Equals("Bird") OrElse Me.level.Equals("Dragon") OrElse Me.level.Equals("Devil") Then
			Me.inAnimated.rectTransform.offsetMax = New Vector2(Me.inAnimated.rectTransform.offsetMax.x, -160F)
		ElseIf Me.level.Equals("Flower") Then
			Me.inAnimated.rectTransform.offsetMax = New Vector2(Me.inAnimated.rectTransform.offsetMax.x, -140F)
		ElseIf Me.level.Equals("Bee") Then
			Me.inAnimated.rectTransform.offsetMax = New Vector2(Me.inAnimated.rectTransform.offsetMax.x, -130F)
		ElseIf Me.level.Equals("KingDiceTop") Then
			Me.inAnimated.rectTransform.offsetMax = New Vector2(Me.inAnimated.rectTransform.offsetMax.x, -155F)
		ElseIf Me.level.Equals("Frogs") OrElse Me.level.Equals("FlyingBlimp") OrElse Me.level.Equals("Baroness") OrElse Me.level.Equals("FlyingGenie") OrElse Me.level.Equals("Clown") OrElse Me.level.Equals("SallyStagePlay") OrElse Me.level.Equals("FlyingMermaid") Then
			Me.inAnimated.rectTransform.offsetMax = New Vector2(Me.inAnimated.rectTransform.offsetMax.x, -110F)
		Else
			Me.inAnimated.rectTransform.offsetMax = New Vector2(Me.inAnimated.rectTransform.offsetMax.x, -150F)
		End If
	End Sub

	' Token: 0x06003998 RID: 14744 RVA: 0x0020B954 File Offset: 0x00209D54
	Private Sub InWordSetup()
		If PlayerData.Data.CurrentMap = Scenes.scene_map_world_DLC Then
			Me.inAnimated.enabled = False
			Me.inText.enabled = False
			Return
		End If
		If Localization.language = Localization.Languages.English Then
			Me.inAnimated.sprite = Me.inSprites(Global.UnityEngine.Random.Range(0, Me.inSprites.Length))
		End If
		Me.inAnimated.enabled = Localization.language <> Localization.Languages.Korean AndAlso Localization.language <> Localization.Languages.SimplifiedChinese AndAlso PlayerData.Data.CurrentMap <> Scenes.scene_map_world_DLC
		Me.inAnimated.transform.localScale = Vector3.one
		If Localization.language = Localization.Languages.French Then
			Me.inAnimated.transform.localScale = Vector3.one * 1.5F
		ElseIf Localization.language = Localization.Languages.PortugueseBrazil OrElse Localization.language = Localization.Languages.SpanishSpain OrElse Localization.language = Localization.Languages.SpanishAmerica Then
			Me.inAnimated.transform.localScale = Vector3.one * 1.2F
		ElseIf Localization.language = Localization.Languages.Russian Then
			Me.inAnimated.transform.localScale = Vector3.one * 2F
		End If
	End Sub

	' Token: 0x0400412D RID: 16685
	Private Const KoreanUpscaleSize As Integer = 100

	' Token: 0x0400412E RID: 16686
	Private Const AsianImageScale As Single = 0.9F

	' Token: 0x0400412F RID: 16687
	Private Const KoreanBossTitleScale As Single = 1.2F

	' Token: 0x04004130 RID: 16688
	Private Const KoreanDifficultyFontSize As Integer = 29

	' Token: 0x04004131 RID: 16689
	Private Const KoreanDifficultyOptionsFontSize As Integer = 37

	' Token: 0x04004134 RID: 16692
	<SerializeField()>
	Private inAnimated As Image

	' Token: 0x04004135 RID: 16693
	<SerializeField()>
	Private bossTitleImage As Image

	' Token: 0x04004136 RID: 16694
	<SerializeField()>
	Private bossNameImage As Image

	' Token: 0x04004137 RID: 16695
	<SerializeField()>
	Private difficultyImage As Image

	' Token: 0x04004138 RID: 16696
	<SerializeField()>
	Private inSprites As Sprite()

	' Token: 0x04004139 RID: 16697
	<SerializeField()>
	Private separatorsAnimated As Image()

	' Token: 0x0400413A RID: 16698
	<SerializeField()>
	Private separatorsSprites As Sprite()

	' Token: 0x0400413B RID: 16699
	<SerializeField()>
	Private cursor As RectTransform

	' Token: 0x0400413C RID: 16700
	<Header("Options")>
	<SerializeField()>
	Private easy As RectTransform

	' Token: 0x0400413D RID: 16701
	<SerializeField()>
	Private normal As RectTransform

	' Token: 0x0400413E RID: 16702
	<SerializeField()>
	Private normalSeparator As RectTransform

	' Token: 0x0400413F RID: 16703
	<SerializeField()>
	Private hard As RectTransform

	' Token: 0x04004140 RID: 16704
	<SerializeField()>
	Private hardSeparator As RectTransform

	' Token: 0x04004141 RID: 16705
	<SerializeField()>
	Private box As RectTransform

	' Token: 0x04004142 RID: 16706
	<SerializeField()>
	Private selectedColor As Color

	' Token: 0x04004143 RID: 16707
	<SerializeField()>
	Private unselectedColor As Color

	' Token: 0x04004144 RID: 16708
	<Header("Stage")>
	<SerializeField()>
	Private bossImage As LocalizationHelper

	' Token: 0x04004145 RID: 16709
	<SerializeField()>
	Private bossName As LocalizationHelper

	' Token: 0x04004146 RID: 16710
	<SerializeField()>
	Private difficultyText As LocalizationHelper

	' Token: 0x04004147 RID: 16711
	<SerializeField()>
	Private bossCardWhiteMaterial As Material

	' Token: 0x04004148 RID: 16712
	<SerializeField()>
	Private difficultySelectionText As Image

	' Token: 0x04004149 RID: 16713
	<SerializeField()>
	Private inText As Text

	' Token: 0x0400414A RID: 16714
	<Header("Glow")>
	<SerializeField()>
	Private glowScript As GlowText

	' Token: 0x0400414B RID: 16715
	<SerializeField()>
	Private bossGlow As LocalizationHelper

	' Token: 0x0400414C RID: 16716
	<SerializeField()>
	Private asianGlow As Image

	' Token: 0x0400414D RID: 16717
	Private difficulyTexts As TMP_Text()

	' Token: 0x0400414E RID: 16718
	Private options As Level.Mode()

	' Token: 0x0400414F RID: 16719
	Private index As Integer = 1

	' Token: 0x04004150 RID: 16720
	Private cursorY As Single

	' Token: 0x04004151 RID: 16721
	Private initialMaxFontSize As Integer

	' Token: 0x04004152 RID: 16722
	Private initialinImagePosX As Vector2

	' Token: 0x04004153 RID: 16723
	Private initialinImagePosY As Vector2

	' Token: 0x04004154 RID: 16724
	Private initialinDifficultyPos As Vector2

	' Token: 0x04004155 RID: 16725
	Private initialDifficultyPos As Vector2

	' Token: 0x04004156 RID: 16726
	Private initialBossNamePos As Vector2
End Class
