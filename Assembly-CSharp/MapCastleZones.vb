Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000932 RID: 2354
Public Class MapCastleZones
	Inherits MonoBehaviour

	' Token: 0x06003710 RID: 14096 RVA: 0x001FB800 File Offset: 0x001F9C00
	Private Sub OnEnable()
		For Each mapCastleZoneCollider As MapCastleZoneCollider In Me.zones
			AddHandler mapCastleZoneCollider.OnMapCastleZoneCollision, AddressOf Me.onMapCastleZoneCollision
		Next
	End Sub

	' Token: 0x06003711 RID: 14097 RVA: 0x001FB840 File Offset: 0x001F9C40
	Private Sub OnDisable()
		For Each mapCastleZoneCollider As MapCastleZoneCollider In Me.zones
			RemoveHandler mapCastleZoneCollider.OnMapCastleZoneCollision, AddressOf Me.onMapCastleZoneCollision
		Next
	End Sub

	' Token: 0x06003712 RID: 14098 RVA: 0x001FB87E File Offset: 0x001F9C7E
	Private Sub showLadder(zone As MapCastleZoneCollider)
		Me.ladder.transform.position = zone.interactionPoint.position
		Me.ladder.returnPositions = zone.returnPositions
		MyBase.StartCoroutine(Me.showLadder_cr(zone))
	End Sub

	' Token: 0x06003713 RID: 14099 RVA: 0x001FB8BC File Offset: 0x001F9CBC
	Private Iterator Function showLadder_cr(zone As MapCastleZoneCollider) As IEnumerator
		Me.ladder.animator.SetBool("Down", True)
		Yield Me.ladder.animator.WaitForAnimationToStart(Me, "Drop", False)
		AudioManager.Play("worldmap_kog_ladder_down")
		Me.ladder.EnableShadow(zone.enableLadderShadow)
		Yield Me.ladder.animator.WaitForAnimationToEnd(Me, "Drop", False, True)
		Me.ladder.enabled = True
		Return
	End Function

	' Token: 0x06003714 RID: 14100 RVA: 0x001FB8DE File Offset: 0x001F9CDE
	Private Sub hideLadder()
		MyBase.StartCoroutine(Me.hideLadder_cr())
	End Sub

	' Token: 0x06003715 RID: 14101 RVA: 0x001FB8F0 File Offset: 0x001F9CF0
	Private Iterator Function hideLadder_cr() As IEnumerator
		Me.ladder.animator.SetBool("Down", False)
		Yield Me.ladder.animator.WaitForAnimationToStart(Me, "Up", False)
		AudioManager.Play("worldmap_kog_ladder_up")
		Me.ladder.enabled = False
		Return
	End Function

	' Token: 0x06003716 RID: 14102 RVA: 0x001FB90C File Offset: 0x001F9D0C
	Private Sub onMapCastleZoneCollision(collider As MapCastleZoneCollider, other As GameObject, phase As CollisionPhase)
		If phase = CollisionPhase.Enter Then
			If Me.currentZone Is Nothing Then
				Dim data As PlayerData = PlayerData.Data
				If data.CountLevelsCompleted(Level.kingOfGamesLevels) = Level.kingOfGamesLevels.Length Then
					If collider.zone = MapCastleZones.Zone.Dock Then
						Me.currentZone = collider
					End If
				ElseIf collider.zone <> MapCastleZones.Zone.Dock Then
					If data.currentChessBossZone = MapCastleZones.Zone.None Then
						Dim num As Integer = data.CountLevelsCompleted(Level.worldDLCBossLevels)
						Dim count As Integer = data.usedChessBossZones.Count
						If count <= num AndAlso Not data.usedChessBossZones.Contains(collider.zone) Then
							Me.currentZone = collider
							data.currentChessBossZone = collider.zone
							PlayerData.SaveCurrentFile()
						End If
					ElseIf data.currentChessBossZone = collider.zone Then
						Me.currentZone = collider
					End If
				End If
				If Me.currentZone IsNot Nothing Then
					Me.showLadder(Me.currentZone)
				End If
			End If
			If collider Is Me.currentZone Then
				Me.currentZonePlayerCount += 1
			End If
		ElseIf phase = CollisionPhase.[Exit] AndAlso collider Is Me.currentZone Then
			Me.currentZonePlayerCount -= 1
			If Me.currentZonePlayerCount = 0 Then
				Me.currentZone = Nothing
				Me.hideLadder()
			End If
		End If
	End Sub

	' Token: 0x04003F3F RID: 16191
	Private Shared RegularZones As MapCastleZones.Zone() = New MapCastleZones.Zone() { MapCastleZones.Zone.OldMan, MapCastleZones.Zone.RumRunners, MapCastleZones.Zone.Cowgirl, MapCastleZones.Zone.DogFight, MapCastleZones.Zone.SnowCult }

	' Token: 0x04003F40 RID: 16192
	<SerializeField()>
	Private zones As MapCastleZoneCollider()

	' Token: 0x04003F41 RID: 16193
	<SerializeField()>
	Private ladder As MapLevelLoaderLadder

	' Token: 0x04003F42 RID: 16194
	Private currentZone As MapCastleZoneCollider

	' Token: 0x04003F43 RID: 16195
	Private currentZonePlayerCount As Integer

	' Token: 0x02000933 RID: 2355
	Public Enum Zone
		' Token: 0x04003F45 RID: 16197
		None
		' Token: 0x04003F46 RID: 16198
		OldMan
		' Token: 0x04003F47 RID: 16199
		RumRunners
		' Token: 0x04003F48 RID: 16200
		Cowgirl
		' Token: 0x04003F49 RID: 16201
		DogFight
		' Token: 0x04003F4A RID: 16202
		SnowCult
		' Token: 0x04003F4B RID: 16203
		Dock
	End Enum
End Class
