Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020007B4 RID: 1972
Public Class SallyStagePlayLevelRose
	Inherits AbstractProjectile

	' Token: 0x06002C66 RID: 11366 RVA: 0x001A1840 File Offset: 0x0019FC40
	Public Function Create(pos As Vector2, properties As LevelProperties.SallyStagePlay.Roses) As SallyStagePlayLevelRose
		Dim sallyStagePlayLevelRose As SallyStagePlayLevelRose = TryCast(MyBase.Create(pos), SallyStagePlayLevelRose)
		sallyStagePlayLevelRose.properties = properties
		Return sallyStagePlayLevelRose
	End Function

	' Token: 0x06002C67 RID: 11367 RVA: 0x001A1864 File Offset: 0x0019FC64
	Protected Overrides Sub Start()
		MyBase.Start()
		MyBase.transform.SetScale(New Single?(CSng(If((Not Rand.Bool()), (-1), 1))), Nothing, Nothing)
		MyBase.StartCoroutine(Me.move_cr())
	End Sub

	' Token: 0x06002C68 RID: 11368 RVA: 0x001A18B8 File Offset: 0x0019FCB8
	Protected Overrides Sub Update()
		MyBase.Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x06002C69 RID: 11369 RVA: 0x001A18D6 File Offset: 0x0019FCD6
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x06002C6A RID: 11370 RVA: 0x001A18F4 File Offset: 0x0019FCF4
	Private Iterator Function move_cr() As IEnumerator
		Dim speed As Single = Me.properties.fallSpeed.min
		While MyBase.transform.position.y > CSng((Level.Current.Ground + 10))
			MyBase.transform.position += Vector3.down * speed * CupheadTime.Delta
			If speed < Me.properties.fallSpeed.max Then
				speed += Me.properties.fallAcceleration
			End If
			Yield Nothing
		End While
		MyBase.animator.SetTrigger("Land")
		MyBase.GetComponent(Of BoxCollider2D)().enabled = False
		MyBase.animator.SetBool("IsA", Rand.Bool())
		Yield CupheadTime.WaitForSeconds(Me, Me.properties.groundDuration)
		MyBase.StartCoroutine(Me.despawn_cr())
		Return
	End Function

	' Token: 0x06002C6B RID: 11371 RVA: 0x001A1910 File Offset: 0x0019FD10
	Private Iterator Function despawn_cr() As IEnumerator
		Dim s As SpriteRenderer = MyBase.GetComponentInChildren(Of SpriteRenderer)(False)
		Dim t As Single = 0F
		Dim time As Single = 2F
		While t < time
			t += CupheadTime.Delta
			s.color = New Color(s.color.r, s.color.b, s.color.g, 1F - t / time)
			Yield Nothing
		End While
		MyBase.GetComponent(Of Collider2D)().enabled = False
		Me.Die()
		Return
	End Function

	' Token: 0x06002C6C RID: 11372 RVA: 0x001A192B File Offset: 0x0019FD2B
	Protected Overrides Sub Die()
		Me.StopAllCoroutines()
		MyBase.GetComponentInChildren(Of SpriteRenderer)(False).enabled = False
		MyBase.Die()
	End Sub

	' Token: 0x06002C6D RID: 11373 RVA: 0x001A1948 File Offset: 0x0019FD48
	Public Overrides Sub SetParryable(parryable As Boolean)
		MyBase.SetParryable(parryable)
		Me.pinkRose.SetActive(False)
		Me.normalRose.SetActive(False)
		If parryable Then
			Me.pinkRose.SetActive(True)
		Else
			Me.normalRose.SetActive(True)
		End If
	End Sub

	' Token: 0x040034FC RID: 13564
	<SerializeField()>
	Private normalRose As GameObject

	' Token: 0x040034FD RID: 13565
	<SerializeField()>
	Private pinkRose As GameObject

	' Token: 0x040034FE RID: 13566
	Private properties As LevelProperties.SallyStagePlay.Roses

	' Token: 0x040034FF RID: 13567
	Private speed As Single
End Class
