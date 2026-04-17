Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000A52 RID: 2642
Public Class PlayerSuperChaliceIII
	Inherits AbstractPlayerSuper

	' Token: 0x06003EF7 RID: 16119 RVA: 0x00227E54 File Offset: 0x00226254
	Protected Overrides Sub StartSuper()
		MyBase.StartSuper()
		If Not Me.player.motor.Grounded Then
			MyBase.animator.Play("StartAir")
		End If
		MyBase.animator.Update(0F)
		Me.minionSpawn = New PatternString(Me.minionSpawnString, True)
		Me.minionSpawnTiming = New PatternString(Me.minionSpawnTimingString, True)
		Me.direction = MyBase.transform.localScale.x
		Me.zoomFactor = Camera.main.orthographicSize / 360F
		AudioManager.Play("player_super_chalice_barrage_start")
	End Sub

	' Token: 0x06003EF8 RID: 16120 RVA: 0x00227EFC File Offset: 0x002262FC
	Private Iterator Function super_cr() As IEnumerator
		Me.Fire()
		While True
			If Me.player IsNot Nothing Then
				If Me.target.position.y < Me.player.transform.position.y Then
					Me.target.position = New Vector3(Me.target.position.x, Me.player.transform.position.y)
				Else
					Me.target.position += Vector3.down * Me.aimSinkRate * CupheadTime.Delta
				End If
			Else
				Me.EndSuper(True)
				Me.StopAllCoroutines()
			End If
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06003EF9 RID: 16121 RVA: 0x00227F17 File Offset: 0x00226317
	Private Sub StartMinions()
		MyBase.StartCoroutine(Me.shoot_cr())
		MyBase.StartCoroutine(Me.super_cr())
	End Sub

	' Token: 0x06003EFA RID: 16122 RVA: 0x00227F34 File Offset: 0x00226334
	Private Iterator Function shoot_cr() As IEnumerator
		Me.mainTimer = WeaponProperties.LevelSuperChaliceIII.superDuration
		Yield Nothing
		For i As Integer = 0 To Me.minionCount - 1
			Dim spawnDataOffset As Integer = Me.minionSpawn.GetSubStringIndex()
			Dim spawnType As Integer = Me.minionSpawn.PopInt()
			Dim angle As Single = If((Me.direction <= 0F), 180F, 0F)
			Dim bullet As BasicProjectile = Me.minionPrefab.Create(New Vector3(CupheadLevelCamera.Current.transform.position.x - 1000F * Mathf.Sign(Me.direction), Me.target.position.y + Me.minionVerticalRange(spawnDataOffset).RandomFloat() * If((Not Me.linkRangeToZoom), 1F, Me.zoomFactor)), angle, Me.minionSpeed(spawnDataOffset) * If((Not Me.linkSpeedToZoom), 1F, Me.zoomFactor))
			bullet.Damage = Me.minionDamage(spawnDataOffset) / If((Not Me.linkDamageToZoom), 1F, Me.zoomFactor)
			Dim s As Single = Me.minionScaleRange(spawnDataOffset).RandomFloat() * If((Not Me.linkScaleToZoom), 1F, Me.zoomFactor)
			bullet.transform.localScale = New Vector3(s, s)
			CType(bullet, PlayerSuperChaliceIIIMinion).elementIndex = spawnDataOffset
			CType(bullet, PlayerSuperChaliceIIIMinion).wave = Me.wave
			Dim r As SpriteRenderer = bullet.GetComponent(Of SpriteRenderer)()
			r.flipY = Me.direction < 0F
			r.sortingOrder = If((s >= 1F), (100 - spawnType), (-100 - spawnType))
			bullet.transform.position = New Vector3(bullet.transform.position.x, bullet.transform.position.y, (s - 1F) * 0.0001F)
			bullet.GetComponent(Of Animator)().Play(spawnType.ToString())
			bullet.DamageRate = 0.2F
			bullet.PlayerId = Me.player.id
			bullet.GetComponent(Of Collider2D)().isTrigger = True
			Me.minionTypeCount(spawnType) = (Me.minionTypeCount(spawnType) + 1) Mod 3
			Yield CupheadTime.WaitForSeconds(Me, Me.minionSpawnTiming.PopFloat() * If((Not Me.linkSpawnRateToZoom), 1F, (Me.zoomFactor * Me.spawnRateZoomModifier)))
		Next
		While Me.spear AndAlso Not Me.spear.gameObject.activeInHierarchy
			Yield Nothing
		End While
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
		Return
	End Function

	' Token: 0x06003EFB RID: 16123 RVA: 0x00227F50 File Offset: 0x00226350
	Private Sub RespawnChalice()
		Me.EndSuper(True)
		Me.fxRenderer.sortingLayerName = "Player"
		Me.fxRenderer.sortingOrder = -10
		Me.chaliceSprite.sortingLayerName = "Player"
		Me.chaliceSprite.sortingOrder = -20
		Me.chaliceRespawned = True
	End Sub

	' Token: 0x06003EFC RID: 16124 RVA: 0x00227FA8 File Offset: 0x002263A8
	Private Sub ActivateSpear()
		If Me.player IsNot Nothing Then
			Me.spear.DetachFromSuper(Me.player)
		Else
			Me.spear.transform.parent = Nothing
		End If
		Me.spear.gameObject.SetActive(True)
	End Sub

	' Token: 0x040045FB RID: 17915
	<SerializeField()>
	Private minionPrefab As BasicProjectile

	' Token: 0x040045FC RID: 17916
	Private mainTimer As Single = 100F

	' Token: 0x040045FD RID: 17917
	Private direction As Single

	' Token: 0x040045FE RID: 17918
	<SerializeField()>
	Private minionCount As Integer = 50

	' Token: 0x040045FF RID: 17919
	<SerializeField()>
	Private wave As Boolean = True

	' Token: 0x04004600 RID: 17920
	<SerializeField()>
	Private aimSinkRate As Single = 100F

	' Token: 0x04004601 RID: 17921
	<SerializeField()>
	Private chaliceSprite As SpriteRenderer

	' Token: 0x04004602 RID: 17922
	<SerializeField()>
	Private fxRenderer As SpriteRenderer

	' Token: 0x04004603 RID: 17923
	<SerializeField()>
	Private minionVerticalRange As MinMax()

	' Token: 0x04004604 RID: 17924
	<SerializeField()>
	Private minionScaleRange As MinMax()

	' Token: 0x04004605 RID: 17925
	<SerializeField()>
	Private minionSpeed As Single()

	' Token: 0x04004606 RID: 17926
	<SerializeField()>
	Private minionDamage As Single()

	' Token: 0x04004607 RID: 17927
	<SerializeField()>
	Private minionSpawnString As String

	' Token: 0x04004608 RID: 17928
	Private minionSpawn As PatternString

	' Token: 0x04004609 RID: 17929
	<SerializeField()>
	Private minionSpawnTimingString As String

	' Token: 0x0400460A RID: 17930
	Private minionSpawnTiming As PatternString

	' Token: 0x0400460B RID: 17931
	<SerializeField()>
	Private minionTypeCount As Integer() = New Integer(5) {}

	' Token: 0x0400460C RID: 17932
	<SerializeField()>
	Private spear As PlayerSuperChaliceIIISpear

	' Token: 0x0400460D RID: 17933
	<SerializeField()>
	Private target As Transform

	' Token: 0x0400460E RID: 17934
	Private zoomFactor As Single

	' Token: 0x0400460F RID: 17935
	<SerializeField()>
	Private linkSpeedToZoom As Boolean = True

	' Token: 0x04004610 RID: 17936
	<SerializeField()>
	Private linkScaleToZoom As Boolean

	' Token: 0x04004611 RID: 17937
	<SerializeField()>
	Private linkRangeToZoom As Boolean

	' Token: 0x04004612 RID: 17938
	<SerializeField()>
	Private linkDamageToZoom As Boolean

	' Token: 0x04004613 RID: 17939
	<SerializeField()>
	Private linkSpawnRateToZoom As Boolean

	' Token: 0x04004614 RID: 17940
	<SerializeField()>
	Private spawnRateZoomModifier As Single = 1F

	' Token: 0x04004615 RID: 17941
	Private chaliceRespawned As Boolean
End Class
