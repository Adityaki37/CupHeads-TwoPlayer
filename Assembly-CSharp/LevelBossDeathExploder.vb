Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x0200049D RID: 1181
Public Class LevelBossDeathExploder
	Inherits AbstractMonoBehaviour

	' Token: 0x0600133A RID: 4922 RVA: 0x000AA38F File Offset: 0x000A878F
	Protected Overrides Sub Awake()
		MyBase.Awake()
	End Sub

	' Token: 0x0600133B RID: 4923 RVA: 0x000AA398 File Offset: 0x000A8798
	Protected Overridable Sub Start()
		If Me.ExplosionPrefabOverride Then
			Me.effectPrefab = Me.ExplosionPrefabOverride
		Else
			Me.effectPrefab = Level.Current.LevelResources.levelBossDeathExplosion
		End If
		AddHandler Level.Current.OnBossDeathExplosionsEvent, AddressOf Me.StartExplosion
		AddHandler Level.Current.OnBossDeathExplosionsFalloffEvent, AddressOf Me.OnExplosionsRand
		AddHandler Level.Current.OnBossDeathExplosionsEndEvent, AddressOf Me.StopExplosions
	End Sub

	' Token: 0x0600133C RID: 4924 RVA: 0x000AA420 File Offset: 0x000A8820
	Private Sub OnDestroy()
		Me.ExplosionPrefabOverride = Nothing
		Me.effectPrefab = Nothing
		Try
			RemoveHandler Level.Current.OnBossDeathExplosionsEvent, AddressOf Me.StartExplosion
		Catch
		End Try
		Try
			RemoveHandler Level.Current.OnBossDeathExplosionsFalloffEvent, AddressOf Me.OnExplosionsRand
		Catch
		End Try
		Try
			RemoveHandler Level.Current.OnBossDeathExplosionsEndEvent, AddressOf Me.StopExplosions
		Catch
		End Try
	End Sub

	' Token: 0x0600133D RID: 4925 RVA: 0x000AA4C8 File Offset: 0x000A88C8
	Protected Overrides Sub OnDrawGizmosSelected()
		MyBase.OnDrawGizmosSelected()
		Gizmos.color = Color.yellow
		Gizmos.DrawWireSphere(MyBase.baseTransform.position + Me.offset, Me.radius)
	End Sub

	' Token: 0x0600133E RID: 4926 RVA: 0x000AA505 File Offset: 0x000A8905
	Public Sub StartExplosion()
		Me.StartExplosion(False)
	End Sub

	' Token: 0x0600133F RID: 4927 RVA: 0x000AA50E File Offset: 0x000A890E
	Public Sub StartExplosion(bypassCameraShakeEvent As Boolean)
		If Me Is Nothing OrElse Not MyBase.enabled OrElse Not MyBase.isActiveAndEnabled Then
			Return
		End If
		MyBase.StartCoroutine(Me.go_cr(bypassCameraShakeEvent))
	End Sub

	' Token: 0x06001340 RID: 4928 RVA: 0x000AA541 File Offset: 0x000A8941
	Public Sub OnExplosionsRand()
		Me.state = LevelBossDeathExploder.State.Random
	End Sub

	' Token: 0x06001341 RID: 4929 RVA: 0x000AA54A File Offset: 0x000A894A
	Public Sub StopExplosions()
		Me.StopAllCoroutines()
	End Sub

	' Token: 0x06001342 RID: 4930 RVA: 0x000AA554 File Offset: 0x000A8954
	Private Function GetRandomPoint() As Vector2
		Dim vector As Vector2 = MyBase.transform.position + Me.offset
		Dim vector2 As Vector2 = New Vector2(CSng(Global.UnityEngine.Random.Range(-1, 1)), CSng(Global.UnityEngine.Random.Range(-1, 1)))
		Dim vector3 As Vector2 = vector2.normalized * (Me.radius * Global.UnityEngine.Random.value) * 2F
		vector3.x *= Me.scaleFactor.x
		vector3.y *= Me.scaleFactor.y
		Return vector + vector3
	End Function

	' Token: 0x06001343 RID: 4931 RVA: 0x000AA5F0 File Offset: 0x000A89F0
	Private Iterator Function go_cr(bypassCameraShakeEvent As Boolean) As IEnumerator
		Dim flash As HitFlash = MyBase.GetComponent(Of HitFlash)()
		If Not Me.disableSound Then
			AudioManager.Play("level_explosion_boss_death")
		End If
		While True
			Me.effectPrefab.Create(Me.GetRandomPoint())
			If flash IsNot Nothing Then
				flash.Flash(0.1F)
			End If
			CupheadLevelCamera.Current.Shake(10F, 0.4F, bypassCameraShakeEvent)
			Dim state As LevelBossDeathExploder.State = Me.state
			If state <> LevelBossDeathExploder.State.Random Then
				Yield CupheadTime.WaitForSeconds(Me, Me.STEADY_DELAY)
			Else
				Yield CupheadTime.WaitForSeconds(Me, Global.UnityEngine.Random.Range(Me.MIN_DELAY, Me.MAX_DELAY))
			End If
		End While
		Return
	End Function

	' Token: 0x04001C5C RID: 7260
	Public ExplosionPrefabOverride As Effect

	' Token: 0x04001C5D RID: 7261
	<SerializeField()>
	Private STEADY_DELAY As Single = 0.3F

	' Token: 0x04001C5E RID: 7262
	<SerializeField()>
	Private MIN_DELAY As Single = 0.4F

	' Token: 0x04001C5F RID: 7263
	<SerializeField()>
	Private MAX_DELAY As Single = 1F

	' Token: 0x04001C60 RID: 7264
	Public offset As Vector2 = Vector2.zero

	' Token: 0x04001C61 RID: 7265
	<SerializeField()>
	Private radius As Single = 100F

	' Token: 0x04001C62 RID: 7266
	<SerializeField()>
	Private scaleFactor As Vector2 = New Vector2(1F, 1F)

	' Token: 0x04001C63 RID: 7267
	Private state As LevelBossDeathExploder.State

	' Token: 0x04001C64 RID: 7268
	Protected effectPrefab As Effect

	' Token: 0x04001C65 RID: 7269
	<SerializeField()>
	Private disableSound As Boolean

	' Token: 0x0200049E RID: 1182
	Private Enum State
		' Token: 0x04001C67 RID: 7271
		Steady
		' Token: 0x04001C68 RID: 7272
		Random
	End Enum
End Class
