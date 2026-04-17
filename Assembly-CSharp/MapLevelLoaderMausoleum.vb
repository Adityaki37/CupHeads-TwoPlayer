Imports System
Imports Rewired
Imports UnityEngine

' Token: 0x02000940 RID: 2368
Public Class MapLevelLoaderMausoleum
	Inherits MapLevelLoader

	' Token: 0x06003764 RID: 14180 RVA: 0x001FD9B8 File Offset: 0x001FBDB8
	Protected Overrides Sub Update()
		Select Case Me.interactor
			Case Else
				If MyBase.PlayerWithinDistance(0) Then
					Dim actions As Player = Map.Current.players(0).input.actions
					If actions.GetButton(11) AndAlso actions.GetButton(12) Then
						Me.currentDuration += CupheadTime.Delta
					Else
						Me.currentDuration = 0F
					End If
				End If
			Case AbstractMapInteractiveEntity.Interactor.Mugman
				If MyBase.PlayerWithinDistance(1) Then
					Dim actions2 As Player = Map.Current.players(1).input.actions
					If actions2.GetButton(11) AndAlso actions2.GetButton(12) Then
						Me.currentDuration += CupheadTime.Delta
					Else
						Me.currentDuration = 0F
					End If
				End If
			Case AbstractMapInteractiveEntity.Interactor.Either
				Dim flag As Boolean = False
				If MyBase.PlayerWithinDistance(0) Then
					Dim actions3 As Player = Map.Current.players(0).input.actions
					If actions3.GetButton(11) AndAlso actions3.GetButton(12) Then
						Me.currentDuration += CupheadTime.Delta
						flag = True
					End If
				End If
				If MyBase.PlayerWithinDistance(1) Then
					Dim actions4 As Player = Map.Current.players(1).input.actions
					If actions4.GetButton(11) AndAlso actions4.GetButton(12) Then
						Me.currentDuration += CupheadTime.Delta
						flag = True
					End If
				End If
				If Not flag Then
					Me.currentDuration = 0F
				End If
			Case AbstractMapInteractiveEntity.Interactor.Both
				If Map.Current.players(0) Is Nothing OrElse Map.Current.players(1) Is Nothing Then
					Return
				End If
				If MyBase.PlayerWithinDistance(0) AndAlso MyBase.PlayerWithinDistance(1) Then
					If Map.Current.players(0).input.actions.GetButton(13) Then
						If Map.Current.players(1).input.actions.GetButton(13) Then
							Me.currentDuration += CupheadTime.Delta
						Else
							Me.currentDuration = 0F
						End If
					Else
						Me.currentDuration = 0F
					End If
				End If
		End Select
		If Me.currentDuration >= Me.pressDurationToReEnable Then
			Me.reEnabled = True
		End If
		If Not Me.reEnabled Then
			Return
		End If
		MyBase.Update()
	End Sub

	' Token: 0x04003F78 RID: 16248
	<SerializeField()>
	Private pressDurationToReEnable As Single = 1F

	' Token: 0x04003F79 RID: 16249
	Private reEnabled As Boolean

	' Token: 0x04003F7A RID: 16250
	Private currentDuration As Single
End Class
