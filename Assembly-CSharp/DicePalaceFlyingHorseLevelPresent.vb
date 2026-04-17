Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020005C5 RID: 1477
Public Class DicePalaceFlyingHorseLevelPresent
	Inherits AbstractProjectile

	' Token: 0x06001CD5 RID: 7381 RVA: 0x001083AF File Offset: 0x001067AF
	Public Sub Init(startPos As Vector3, targetPos As Vector3, properties As LevelProperties.DicePalaceFlyingHorse.GiftBombs)
		MyBase.transform.position = startPos
		Me.targetPos = targetPos
		Me.properties = properties
		MyBase.StartCoroutine(Me.move_cr())
	End Sub

	' Token: 0x06001CD6 RID: 7382 RVA: 0x001083D8 File Offset: 0x001067D8
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		If Me.damageDealer IsNot Nothing AndAlso phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
		MyBase.OnCollisionPlayer(hit, phase)
	End Sub

	' Token: 0x06001CD7 RID: 7383 RVA: 0x00108401 File Offset: 0x00106801
	Protected Overrides Sub Update()
		MyBase.Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x06001CD8 RID: 7384 RVA: 0x00108420 File Offset: 0x00106820
	Private Iterator Function move_cr() As IEnumerator
		While MyBase.transform.position <> Me.targetPos
			MyBase.transform.position = Vector3.MoveTowards(MyBase.transform.position, Me.targetPos, Me.properties.initialSpeed * CupheadTime.Delta)
			Yield Nothing
		End While
		Yield CupheadTime.WaitForSeconds(Me, Me.properties.explosionTime)
		Dim spreadCountPattern As String() = Me.properties.spreadCount.Split(New Char() { ","c })
		Dim angle As Single = 0F
		Dim parryIndex As Integer = Global.UnityEngine.Random.Range(0, spreadCountPattern.Length)
		For i As Integer = 0 To spreadCountPattern.Length - 1
			Parser.FloatTryParse(spreadCountPattern(i), angle)
			Me.SpawnBullet(angle, parryIndex = i)
		Next
		Yield Nothing
		Me.Die()
		Return
	End Function

	' Token: 0x06001CD9 RID: 7385 RVA: 0x0010843C File Offset: 0x0010683C
	Private Sub SpawnBullet(angle As Single, parryable As Boolean)
		AudioManager.Play("projectile_explo")
		Me.emitAudioFromObject.Add("projectile_explo")
		Dim basicProjectile As BasicProjectile = Me.bullet.Create(MyBase.transform.position, angle, Me.properties.explosionSpeed)
		basicProjectile.SetParryable(parryable)
	End Sub

	' Token: 0x06001CDA RID: 7386 RVA: 0x00108492 File Offset: 0x00106892
	Protected Overrides Sub Die()
		MyBase.GetComponent(Of Collider2D)().enabled = False
		Me.StopAllCoroutines()
		MyBase.Die()
	End Sub

	' Token: 0x040025C1 RID: 9665
	<SerializeField()>
	Private bullet As BasicProjectile

	' Token: 0x040025C2 RID: 9666
	Private properties As LevelProperties.DicePalaceFlyingHorse.GiftBombs

	' Token: 0x040025C3 RID: 9667
	Private targetPos As Vector3
End Class
