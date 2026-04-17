Imports System
Imports System.Collections.Generic
Imports UnityEngine
Imports UnityEngine.UI

' Token: 0x0200099A RID: 2458
Public Class MapConfirmStartUI
	Inherits AbstractMapSceneStartUI

	' Token: 0x170004AA RID: 1194
	' (get) Token: 0x0600397E RID: 14718 RVA: 0x0020A055 File Offset: 0x00208455
	' (set) Token: 0x0600397F RID: 14719 RVA: 0x0020A05C File Offset: 0x0020845C
	Public Shared Property Current As MapConfirmStartUI

	' Token: 0x06003980 RID: 14720 RVA: 0x0020A064 File Offset: 0x00208464
	Protected Overrides Sub Awake()
		MyBase.Awake()
		MapConfirmStartUI.Current = Me
	End Sub

	' Token: 0x06003981 RID: 14721 RVA: 0x0020A074 File Offset: 0x00208474
	Private Sub UpdateCursor()
		Me.cursor.transform.position = Me.enter.transform.position
		Me.cursor.sizeDelta = New Vector2(Me.enter.sizeDelta.x + 30F, Me.enter.sizeDelta.y + 20F)
	End Sub

	' Token: 0x06003982 RID: 14722 RVA: 0x0020A0E3 File Offset: 0x002084E3
	Private Sub OnDestroy()
		If MapConfirmStartUI.Current Is Me Then
			MapConfirmStartUI.Current = Nothing
		End If
	End Sub

	' Token: 0x06003983 RID: 14723 RVA: 0x0020A0FB File Offset: 0x002084FB
	Private Sub Update()
		Me.UpdateCursor()
		If MyBase.CurrentState = AbstractMapSceneStartUI.State.Active Then
			Me.CheckInput()
		End If
	End Sub

	' Token: 0x06003984 RID: 14724 RVA: 0x0020A115 File Offset: 0x00208515
	Private Sub CheckInput()
		If Not MyBase.Able Then
			Return
		End If
		If MyBase.GetButtonDown(CupheadButton.Cancel) Then
			MyBase.Out()
		End If
		If MyBase.GetButtonDown(CupheadButton.Accept) Then
			MyBase.LoadLevel()
		End If
	End Sub

	' Token: 0x06003985 RID: 14725 RVA: 0x0020A14C File Offset: 0x0020854C
	Public Sub InitUI(level As String)
		Dim translationElement As TranslationElement = Localization.Find(level)
		If translationElement IsNot Nothing Then
			Me.Title.ApplyTranslation(translationElement, Nothing)
		End If
		Me.EmptyCoin2.enabled = True
		Me.Coin2.enabled = False
		Me.EmptyCoin3.enabled = True
		Me.Coin3.enabled = False
		Me.EmptyCoin4.enabled = True
		Me.Coin4.enabled = False
		Me.EmptyCoin5.enabled = True
		Me.Coin5.enabled = False
		Dim levelsAndCoins As List(Of PlayerData.PlayerCoinManager.LevelAndCoins) = PlayerData.Data.coinManager.LevelsAndCoins
		For i As Integer = 0 To levelsAndCoins.Count - 1
			If levelsAndCoins(i).level.ToString() = level Then
				If levelsAndCoins(i).Coin1Collected Then
					Me.EmptyCoin1.enabled = False
					Me.Coin1.enabled = True
				Else
					Me.EmptyCoin1.enabled = True
					Me.Coin1.enabled = False
				End If
				If levelsAndCoins(i).Coin2Collected Then
					Me.EmptyCoin2.enabled = False
					Me.Coin2.enabled = True
				Else
					Me.EmptyCoin2.enabled = True
					Me.Coin2.enabled = False
				End If
				If levelsAndCoins(i).Coin3Collected Then
					Me.EmptyCoin3.enabled = False
					Me.Coin3.enabled = True
				Else
					Me.EmptyCoin3.enabled = True
					Me.Coin3.enabled = False
				End If
				If levelsAndCoins(i).Coin4Collected Then
					Me.EmptyCoin4.enabled = False
					Me.Coin4.enabled = True
				Else
					Me.EmptyCoin4.enabled = True
					Me.Coin4.enabled = False
				End If
				If levelsAndCoins(i).Coin5Collected Then
					Me.EmptyCoin5.enabled = False
					Me.Coin5.enabled = True
				Else
					Me.EmptyCoin5.enabled = True
					Me.Coin5.enabled = False
				End If
			End If
		Next
	End Sub

	' Token: 0x06003986 RID: 14726 RVA: 0x0020A37A File Offset: 0x0020877A
	Public Sub [In](playerController As MapPlayerController)
		MyBase.[In](playerController)
		If Me.Animator IsNot Nothing Then
			Me.Animator.SetTrigger("ZoomIn")
			AudioManager.Play("world_map_level_menu_open")
		End If
		Me.InitUI(Me.level)
	End Sub

	' Token: 0x0400411F RID: 16671
	Public Animator As Animator

	' Token: 0x04004120 RID: 16672
	Public Title As LocalizationHelper

	' Token: 0x04004121 RID: 16673
	<Header("Coins")>
	Public EmptyCoin1 As Image

	' Token: 0x04004122 RID: 16674
	Public Coin1 As Image

	' Token: 0x04004123 RID: 16675
	Public EmptyCoin2 As Image

	' Token: 0x04004124 RID: 16676
	Public Coin2 As Image

	' Token: 0x04004125 RID: 16677
	Public EmptyCoin3 As Image

	' Token: 0x04004126 RID: 16678
	Public Coin3 As Image

	' Token: 0x04004127 RID: 16679
	Public EmptyCoin4 As Image

	' Token: 0x04004128 RID: 16680
	Public Coin4 As Image

	' Token: 0x04004129 RID: 16681
	Public EmptyCoin5 As Image

	' Token: 0x0400412A RID: 16682
	Public Coin5 As Image

	' Token: 0x0400412B RID: 16683
	<SerializeField()>
	Private cursor As RectTransform

	' Token: 0x0400412C RID: 16684
	<Header("Options")>
	<SerializeField()>
	Private enter As RectTransform
End Class
