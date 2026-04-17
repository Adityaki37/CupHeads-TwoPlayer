Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020006D7 RID: 1751
Public Class MausoleumLevelGhostBase
	Inherits BasicProjectile

	' Token: 0x170003BC RID: 956
	' (get) Token: 0x06002546 RID: 9542 RVA: 0x0015CD95 File Offset: 0x0015B195
	' (set) Token: 0x06002547 RID: 9543 RVA: 0x0015CD9D File Offset: 0x0015B19D
	Public Property isDead As Boolean

	' Token: 0x170003BD RID: 957
	' (get) Token: 0x06002548 RID: 9544 RVA: 0x0015CDA6 File Offset: 0x0015B1A6
	Protected Overrides ReadOnly Property DestroyLifetime As Single
		Get
			Return 0F
		End Get
	End Property

	' Token: 0x170003BE RID: 958
	' (get) Token: 0x06002549 RID: 9545 RVA: 0x0015CDAD File Offset: 0x0015B1AD
	Public Overrides ReadOnly Property ParryMeterMultiplier As Single
		Get
			Return 0F
		End Get
	End Property

	' Token: 0x0600254A RID: 9546 RVA: 0x0015CDB4 File Offset: 0x0015B1B4
	Protected Overrides Sub Start()
		MyBase.Start()
		Me.isDead = False
		If MyBase.transform.position.x > 0F Then
			MyBase.GetComponent(Of SpriteRenderer)().flipY = True
		End If
		Me.SetParryable(True)
		If Me.Counts Then
			MausoleumLevel.SPAWNCOUNTER += 1
		End If
		MyBase.StartCoroutine(Me.check_dist_cr())
		If Me.hasIdleSFX Then
		End If
	End Sub

	' Token: 0x0600254B RID: 9547 RVA: 0x0015CE2D File Offset: 0x0015B22D
	Public Sub GetParent(parent As MausoleumLevel)
		Me.parent = parent
	End Sub

	' Token: 0x0600254C RID: 9548 RVA: 0x0015CE36 File Offset: 0x0015B236
	Public Overrides Sub OnParry(player As AbstractPlayerController)
		Me.Die()
	End Sub

	' Token: 0x0600254D RID: 9549 RVA: 0x0015CE3E File Offset: 0x0015B23E
	Public Sub OnBossDeath()
		Me.Die()
	End Sub

	' Token: 0x0600254E RID: 9550 RVA: 0x0015CE46 File Offset: 0x0015B246
	Protected Overrides Sub Die()
		Me.StopAllCoroutines()
		Me.isDead = True
		MyBase.Die()
		MyBase.GetComponent(Of SpriteRenderer)().enabled = False
	End Sub

	' Token: 0x0600254F RID: 9551 RVA: 0x0015CE67 File Offset: 0x0015B267
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
	End Sub

	' Token: 0x06002550 RID: 9552 RVA: 0x0015CE70 File Offset: 0x0015B270
	Private Iterator Function check_dist_cr() As IEnumerator
		While Vector3.Distance(MyBase.transform.position, MausoleumLevelUrn.URN_POS) > 30F
			Yield Nothing
		End While
		If Me.parent.LoseGame IsNot Nothing Then
			Me.parent.LoseGame()
		End If
		Yield Nothing
		Return
	End Function

	' Token: 0x06002551 RID: 9553 RVA: 0x0015CE8C File Offset: 0x0015B28C
	Private Iterator Function idle_sound_cr() As IEnumerator
		While True
			Yield CupheadTime.WaitForSeconds(Me, Me.idleDelay.RandomFloat())
			AudioManager.Play(Me.idleSound)
			Me.emitAudioFromObject.Add(Me.idleSound)
			While AudioManager.CheckIfPlaying(Me.idleSound)
				Yield Nothing
			End While
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06002552 RID: 9554 RVA: 0x0015CEA7 File Offset: 0x0015B2A7
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
	End Sub

	' Token: 0x04002DE3 RID: 11747
	<SerializeField()>
	Private idleDelay As MinMax

	' Token: 0x04002DE4 RID: 11748
	<SerializeField()>
	Private idleSound As String

	' Token: 0x04002DE5 RID: 11749
	<SerializeField()>
	Private hasIdleSFX As Boolean

	' Token: 0x04002DE6 RID: 11750
	Public Counts As Boolean

	' Token: 0x04002DE8 RID: 11752
	Private Const DIST_TO_DIE As Single = 30F

	' Token: 0x04002DE9 RID: 11753
	Protected parent As MausoleumLevel
End Class
