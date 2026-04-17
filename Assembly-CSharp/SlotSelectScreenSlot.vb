Imports System
Imports System.Collections
Imports TMPro
Imports UnityEngine
Imports UnityEngine.UI

' Token: 0x020009AF RID: 2479
Public Class SlotSelectScreenSlot
	Inherits AbstractMonoBehaviour

	' Token: 0x170004B6 RID: 1206
	' (get) Token: 0x06003A18 RID: 14872 RVA: 0x002109F8 File Offset: 0x0020EDF8
	' (set) Token: 0x06003A19 RID: 14873 RVA: 0x00210A00 File Offset: 0x0020EE00
	Public Property IsEmpty As Boolean

	' Token: 0x170004B7 RID: 1207
	' (get) Token: 0x06003A1A RID: 14874 RVA: 0x00210A09 File Offset: 0x0020EE09
	' (set) Token: 0x06003A1B RID: 14875 RVA: 0x00210A11 File Offset: 0x0020EE11
	Public Property isPlayer1Mugman As Boolean

	' Token: 0x170004B8 RID: 1208
	' (get) Token: 0x06003A1C RID: 14876 RVA: 0x00210A1A File Offset: 0x0020EE1A
	' (set) Token: 0x06003A1D RID: 14877 RVA: 0x00210A22 File Offset: 0x0020EE22
	Public Property noise As Image
		Get
			Return Me.noiseImage
		End Get
		Set(value As Image)
			Me.noiseImage = value
		End Set
	End Property

	' Token: 0x06003A1E RID: 14878 RVA: 0x00210A2C File Offset: 0x0020EE2C
	Public Sub Init(slotNumber As Integer)
		Dim dataForSlot As PlayerData = PlayerData.GetDataForSlot(slotNumber)
		Me.cuphead.SetActive(False)
		Me.mugman.SetActive(False)
		If Not dataForSlot.GetMapData(Scenes.scene_map_world_1).sessionStarted AndAlso Not dataForSlot.IsTutorialCompleted AndAlso dataForSlot.CountLevelsCompleted(Level.world1BossLevels) = 0 Then
			Me.emptyChild.gameObject.SetActive(True)
			Me.mainChild.gameObject.SetActive(False)
			Me.mainDLCChild.gameObject.SetActive(False)
			Me.IsEmpty = True
			Return
		End If
		Me.IsEmpty = False
		Me.emptyChild.gameObject.SetActive(False)
		Me.mainChild.gameObject.SetActive(True)
		Dim translation As Localization.Translation
		If slotNumber = 0 Then
			If dataForSlot.isPlayer1Mugman Then
				translation = Localization.Translate("TitleScreenMugmanSlot1")
			Else
				translation = Localization.Translate("TitleScreenSlot1")
			End If
		ElseIf slotNumber = 1 Then
			If dataForSlot.isPlayer1Mugman Then
				translation = Localization.Translate("TitleScreenMugmanSlot2")
			Else
				translation = Localization.Translate("TitleScreenSlot2")
			End If
		ElseIf dataForSlot.isPlayer1Mugman Then
			translation = Localization.Translate("TitleScreenMugmanSlot3")
		Else
			translation = Localization.Translate("TitleScreenSlot3")
		End If
		Me.slotTitle.text = translation.text
		Me.slotSeparator.font = translation.fonts.fontAsset
		Me.slotTitle.font = translation.fonts.fontAsset
		Me.isPlayer1Mugman = dataForSlot.isPlayer1Mugman
		Me.isExpert = dataForSlot.IsHardModeAvailable
		Me.isExpertDLC = dataForSlot.IsHardModeAvailableDLC
		Dim num As Integer = Mathf.RoundToInt(dataForSlot.GetCompletionPercentage())
		Me.isComplete = num = 200
		Dim num2 As Integer = Mathf.RoundToInt(dataForSlot.GetCompletionPercentageDLC())
		Me.isCompleteDLC = num2 = 100
		Me.slotPercentage.text = num + num2 + "%"
		If DLCManager.DLCEnabled() Then
			Me.slotPercentageSelectedBase.text = num + "%"
			Me.slotPercentageSelectedDLC.text = num2 + "%"
		End If
		Dim currentMap As Scenes = dataForSlot.CurrentMap
		Select Case currentMap
			Case Scenes.scene_map_world_2
				translation = Localization.Translate("TitleScreenWorld2")
			Case Scenes.scene_map_world_3
				translation = Localization.Translate("TitleScreenWorld3")
			Case Scenes.scene_map_world_4
				translation = Localization.Translate("TitleScreenWorld4")
			Case Else
				If currentMap <> Scenes.scene_map_world_DLC Then
					translation = Localization.Translate("TitleScreenWorld1")
				ElseIf DLCManager.DLCEnabled() Then
					translation = Localization.Translate("TitleScreenWorldDLC")
				Else
					translation = Localization.Translate("TitleScreenWorld1")
				End If
		End Select
		Me.worldMapText.text = translation.text
		Me.worldMapText.font = translation.fonts.fontAsset
		Me.worldMapTextDLC.text = translation.text
		Me.worldMapTextDLC.font = translation.fonts.fontAsset
	End Sub

	' Token: 0x06003A1F RID: 14879 RVA: 0x00210D4C File Offset: 0x0020F14C
	Public Sub SetSelected(selected As Boolean)
		If DLCManager.DLCEnabled() AndAlso Not Me.IsEmpty Then
			Me.mainChild.gameObject.SetActive(Not selected)
			Me.mainDLCChild.gameObject.SetActive(selected)
		End If
		Me.slotTitle.color = If((Not selected), Me.unselectedTextColor, Me.selectedTextColor)
		Me.slotSeparator.color = If((Not selected), Me.unselectedTextColor, Me.selectedTextColor)
		Me.slotPercentage.color = If((Not selected), Me.unselectedTextColor, Me.selectedTextColor)
		Me.worldMapText.color = If((Not selected), Me.unselectedTextColor, Me.selectedTextColor)
		Me.emptyText.color = If((Not selected), Me.unselectedTextColor, Me.selectedTextColor)
		Me.boxImage.sprite = If((Not selected), Me.unselectedBoxSprite, If((Not Me.isPlayer1Mugman), Me.selectedBoxSprite, Me.selectedBoxSpriteMugman))
		If Not Me.IsEmpty AndAlso Me.isComplete Then
			Me.starImage.sprite = If((Not selected), Me.unselectedBoxSpriteComplete, Me.selectedBoxSpriteComplete)
			Me.starImage.gameObject.SetActive(True)
		ElseIf Not Me.IsEmpty AndAlso Me.isExpert Then
			Me.starImage.sprite = If((Not selected), Me.unselectedBoxSpriteExpert, Me.selectedBoxSpriteExpert)
			Me.starImage.gameObject.SetActive(True)
		Else
			Me.starImage.gameObject.SetActive(False)
		End If
		If Not Me.IsEmpty AndAlso Me.isCompleteDLC Then
			Me.starImageDLC.sprite = If((Not selected), Me.unselectedBoxSpriteCompleteDLC, Me.selectedBoxSpriteCompleteDLC)
			Me.starImageDLC.gameObject.SetActive(True)
		ElseIf Not Me.IsEmpty AndAlso Me.isExpertDLC Then
			Me.starImageDLC.sprite = If((Not selected), Me.unselectedBoxSpriteExpertDLC, Me.selectedBoxSpriteExpertDLC)
			Me.starImageDLC.gameObject.SetActive(True)
		Else
			Me.starImageDLC.gameObject.SetActive(False)
		End If
		If Me.starImage.gameObject.activeInHierarchy AndAlso Not Me.starImageDLC.gameObject.activeInHierarchy Then
			Me.starImage.transform.position = Me.starImageDLC.transform.position
		End If
		Me.noiseImage.sprite = If((Not selected), Me.unselectedNoise, If((Not Me.isPlayer1Mugman), Me.selectedNoise, Me.selectedNoiseMugman))
	End Sub

	' Token: 0x06003A20 RID: 14880 RVA: 0x00211051 File Offset: 0x0020F451
	Public Function GetSlotTitle() As String
		Return Me.slotTitle.text
	End Function

	' Token: 0x06003A21 RID: 14881 RVA: 0x0021105E File Offset: 0x0020F45E
	Public Function GetSlotTitleFont() As TMP_FontAsset
		Return Me.slotTitle.font
	End Function

	' Token: 0x06003A22 RID: 14882 RVA: 0x0021106B File Offset: 0x0020F46B
	Public Function GetSlotSeparator() As String
		Return Me.slotSeparator.text
	End Function

	' Token: 0x06003A23 RID: 14883 RVA: 0x00211078 File Offset: 0x0020F478
	Public Function GetSlotSeparatorFont() As TMP_FontAsset
		Return Me.slotSeparator.font
	End Function

	' Token: 0x06003A24 RID: 14884 RVA: 0x00211085 File Offset: 0x0020F485
	Public Function GetSlotPercentage() As String
		Return Me.slotPercentage.text
	End Function

	' Token: 0x06003A25 RID: 14885 RVA: 0x00211092 File Offset: 0x0020F492
	Public Function GetSlotPercentageFont() As TMP_FontAsset
		Return Me.slotPercentage.font
	End Function

	' Token: 0x06003A26 RID: 14886 RVA: 0x002110A0 File Offset: 0x0020F4A0
	Public Sub EnterSelectMenu()
		Me.selectingMugman = Me.isPlayer1Mugman
		If Me.selectingMugman Then
			Me.mugman.SetActive(True)
			Me.mugmanSelect.Play("Zoom_In")
		Else
			Me.cuphead.SetActive(True)
			Me.cupheadSelect.Play("Zoom_In")
		End If
		Me.mainDLCChild.gameObject.SetActive(False)
	End Sub

	' Token: 0x06003A27 RID: 14887 RVA: 0x00211114 File Offset: 0x0020F514
	Public Sub SwapSprite()
		Me.noiseImage.enabled = False
		Me.selectingMugman = Not Me.selectingMugman
		Me.cuphead.SetActive(Not Me.selectingMugman)
		Me.mugman.SetActive(Me.selectingMugman)
	End Sub

	' Token: 0x06003A28 RID: 14888 RVA: 0x00211161 File Offset: 0x0020F561
	Public Sub StopSelectingPlayer()
		MyBase.StartCoroutine(Me.player_zoomout_cr())
	End Sub

	' Token: 0x06003A29 RID: 14889 RVA: 0x00211170 File Offset: 0x0020F570
	Private Iterator Function player_zoomout_cr() As IEnumerator
		If Me.selectingMugman Then
			Me.mugmanSelect.Play("Zoom_Out")
			Yield Me.mugmanSelect.WaitForAnimationToEnd(Me, "Zoom_Out", False, True)
			Me.mugman.SetActive(False)
		Else
			Me.cupheadSelect.Play("Zoom_Out")
			Yield Me.cupheadSelect.WaitForAnimationToEnd(Me, "Zoom_Out", False, True)
			Me.cuphead.SetActive(False)
		End If
		Yield Nothing
		Me.selectingMugman = Me.isPlayer1Mugman
		Me.noiseImage.enabled = True
		Return
	End Function

	' Token: 0x06003A2A RID: 14890 RVA: 0x0021118C File Offset: 0x0020F58C
	Public Sub PlayAnimation(slotNumber As Integer)
		Me.isPlayer1Mugman = Me.selectingMugman
		Dim dataForSlot As PlayerData = PlayerData.GetDataForSlot(slotNumber)
		Dim animator As Animator = If((Not Me.isPlayer1Mugman), Me.cupheadAnimator, Me.mugmanAnimator)
		If dataForSlot.IsHardModeAvailable Then
			If dataForSlot.NumCoinsCollected >= 40 AndAlso dataForSlot.NumSupers(PlayerId.PlayerOne) >= 3 Then
				animator.Play("100Percent")
			Else
				animator.Play("DefeatedDevil")
			End If
		Else
			animator.Play("Default")
		End If
	End Sub

	' Token: 0x04004230 RID: 16944
	<SerializeField()>
	Private emptyChild As RectTransform

	' Token: 0x04004231 RID: 16945
	<SerializeField()>
	Private mainChild As RectTransform

	' Token: 0x04004232 RID: 16946
	<SerializeField()>
	Private mainDLCChild As RectTransform

	' Token: 0x04004233 RID: 16947
	<SerializeField()>
	Private worldMapText As TMP_Text

	' Token: 0x04004234 RID: 16948
	<SerializeField()>
	Private worldMapTextDLC As TMP_Text

	' Token: 0x04004235 RID: 16949
	<SerializeField()>
	Private boxImage As Image

	' Token: 0x04004236 RID: 16950
	<SerializeField()>
	Private starImage As Image

	' Token: 0x04004237 RID: 16951
	<SerializeField()>
	Private starImageDLC As Image

	' Token: 0x04004238 RID: 16952
	<SerializeField()>
	Private starImageSelectedBase As Image

	' Token: 0x04004239 RID: 16953
	<SerializeField()>
	Private starImageSelectedDLC As Image

	' Token: 0x0400423A RID: 16954
	<SerializeField()>
	Private noiseImage As Image

	' Token: 0x0400423B RID: 16955
	<SerializeField()>
	Private unselectedBoxSprite As Sprite

	' Token: 0x0400423C RID: 16956
	<SerializeField()>
	Private unselectedBoxSpriteExpert As Sprite

	' Token: 0x0400423D RID: 16957
	<SerializeField()>
	Private unselectedBoxSpriteComplete As Sprite

	' Token: 0x0400423E RID: 16958
	<SerializeField()>
	Private unselectedBoxSpriteExpertDLC As Sprite

	' Token: 0x0400423F RID: 16959
	<SerializeField()>
	Private unselectedBoxSpriteCompleteDLC As Sprite

	' Token: 0x04004240 RID: 16960
	<SerializeField()>
	Private unselectedNoise As Sprite

	' Token: 0x04004241 RID: 16961
	<SerializeField()>
	Private selectedBoxSpriteMugman As Sprite

	' Token: 0x04004242 RID: 16962
	<SerializeField()>
	Private selectedBoxSprite As Sprite

	' Token: 0x04004243 RID: 16963
	<SerializeField()>
	Private selectedBoxSpriteExpert As Sprite

	' Token: 0x04004244 RID: 16964
	<SerializeField()>
	Private selectedBoxSpriteComplete As Sprite

	' Token: 0x04004245 RID: 16965
	<SerializeField()>
	Private selectedBoxSpriteExpertDLC As Sprite

	' Token: 0x04004246 RID: 16966
	<SerializeField()>
	Private selectedBoxSpriteCompleteDLC As Sprite

	' Token: 0x04004247 RID: 16967
	<SerializeField()>
	Private selectedNoiseMugman As Sprite

	' Token: 0x04004248 RID: 16968
	<SerializeField()>
	Private selectedNoise As Sprite

	' Token: 0x04004249 RID: 16969
	<SerializeField()>
	Private cuphead As GameObject

	' Token: 0x0400424A RID: 16970
	<SerializeField()>
	Private cupheadSelect As Animator

	' Token: 0x0400424B RID: 16971
	<SerializeField()>
	Private cupheadAnimator As Animator

	' Token: 0x0400424C RID: 16972
	<SerializeField()>
	Private mugman As GameObject

	' Token: 0x0400424D RID: 16973
	<SerializeField()>
	Private mugmanSelect As Animator

	' Token: 0x0400424E RID: 16974
	<SerializeField()>
	Private mugmanAnimator As Animator

	' Token: 0x0400424F RID: 16975
	<SerializeField()>
	Private slotTitle As TMP_Text

	' Token: 0x04004250 RID: 16976
	<SerializeField()>
	Private slotSeparator As TMP_Text

	' Token: 0x04004251 RID: 16977
	<SerializeField()>
	Private slotPercentage As TMP_Text

	' Token: 0x04004252 RID: 16978
	<SerializeField()>
	Private slotPercentageSelectedBase As TMP_Text

	' Token: 0x04004253 RID: 16979
	<SerializeField()>
	Private slotPercentageSelectedDLC As TMP_Text

	' Token: 0x04004254 RID: 16980
	<SerializeField()>
	Private emptyText As Text

	' Token: 0x04004255 RID: 16981
	<SerializeField()>
	Private selectedTextColor As Color

	' Token: 0x04004256 RID: 16982
	<SerializeField()>
	Private unselectedTextColor As Color

	' Token: 0x04004259 RID: 16985
	Private selectingMugman As Boolean

	' Token: 0x0400425A RID: 16986
	Private isExpert As Boolean

	' Token: 0x0400425B RID: 16987
	Private isExpertDLC As Boolean

	' Token: 0x0400425C RID: 16988
	Private isComplete As Boolean

	' Token: 0x0400425D RID: 16989
	Private isCompleteDLC As Boolean
End Class
