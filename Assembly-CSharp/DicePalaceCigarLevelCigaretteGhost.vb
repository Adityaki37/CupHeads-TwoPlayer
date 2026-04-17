Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020005AF RID: 1455
Public Class DicePalaceCigarLevelCigaretteGhost
	Inherits AbstractProjectile

	' Token: 0x06001C28 RID: 7208 RVA: 0x0010261C File Offset: 0x00100A1C
	Public Sub InitGhost(properties As LevelProperties.DicePalaceCigar)
		Me.properties = properties
		MyBase.StartCoroutine(Me.move_cr())
	End Sub

	' Token: 0x06001C29 RID: 7209 RVA: 0x00102634 File Offset: 0x00100A34
	Private Iterator Function move_cr() As IEnumerator
		MyBase.StartCoroutine(Me.spawn_fx_cr())
		Dim wait As YieldInstruction = New WaitForFixedUpdate()
		While MyBase.transform.position.y < 560F
			MyBase.transform.AddPosition(0F, Me.properties.CurrentState.cigaretteGhost.verticalSpeed * CupheadTime.FixedDelta, 0F)
			Yield wait
		End While
		Me.Die()
		Return
	End Function

	' Token: 0x06001C2A RID: 7210 RVA: 0x0010264F File Offset: 0x00100A4F
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
		MyBase.OnCollisionPlayer(hit, phase)
	End Sub

	' Token: 0x06001C2B RID: 7211 RVA: 0x0010266D File Offset: 0x00100A6D
	Protected Overrides Sub OnDestroy()
		Me.StopAllCoroutines()
		MyBase.OnDestroy()
	End Sub

	' Token: 0x06001C2C RID: 7212 RVA: 0x0010267B File Offset: 0x00100A7B
	Protected Overrides Sub Die()
		MyBase.Die()
		Me.StopAllCoroutines()
	End Sub

	' Token: 0x06001C2D RID: 7213 RVA: 0x0010268C File Offset: 0x00100A8C
	Private Iterator Function spawn_fx_cr() As IEnumerator
		Dim isVal As Boolean = Rand.Bool()
		While True
			Dim value As Single = Global.UnityEngine.Random.Range(0.4F, 0.6F)
			Dim value2 As Single = Global.UnityEngine.Random.Range(0.2F, 0.3F)
			Dim chosenVal As Single = If((Not isVal), value2, value)
			Yield CupheadTime.WaitForSeconds(Me, chosenVal)
			Dim t As Single = 0F
			Dim time As Single = chosenVal
			While t < time
				t += CupheadTime.Delta
				Me.fx.Create(Me.root.transform.position)
				Yield CupheadTime.WaitForSeconds(Me, 0.1F)
				Yield Nothing
			End While
			Yield CupheadTime.WaitForSeconds(Me, Global.UnityEngine.Random.Range(0.25F, 0.45F))
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x04002537 RID: 9527
	<SerializeField()>
	Private root As Transform

	' Token: 0x04002538 RID: 9528
	<SerializeField()>
	Private fx As Effect

	' Token: 0x04002539 RID: 9529
	Private centerPoint As Vector3

	' Token: 0x0400253A RID: 9530
	Private properties As LevelProperties.DicePalaceCigar
End Class
