Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020006A2 RID: 1698
Public Class FlyingMermaidLevelTurtleCannonBall
	Inherits AbstractProjectile

	' Token: 0x06002400 RID: 9216 RVA: 0x001522E0 File Offset: 0x001506E0
	Public Function Create(pos As Vector2, explodePattern As String, properties As LevelProperties.FlyingMermaid.Turtle) As FlyingMermaidLevelTurtleCannonBall
		Dim flyingMermaidLevelTurtleCannonBall As FlyingMermaidLevelTurtleCannonBall = TryCast(MyBase.Create(), FlyingMermaidLevelTurtleCannonBall)
		flyingMermaidLevelTurtleCannonBall.properties = properties
		flyingMermaidLevelTurtleCannonBall.explodePattern = explodePattern
		flyingMermaidLevelTurtleCannonBall.transform.position = pos
		Return flyingMermaidLevelTurtleCannonBall
	End Function

	' Token: 0x06002401 RID: 9217 RVA: 0x00152319 File Offset: 0x00150719
	Protected Overrides Sub Start()
		MyBase.Start()
		MyBase.StartCoroutine(Me.loop_cr())
	End Sub

	' Token: 0x06002402 RID: 9218 RVA: 0x0015232E File Offset: 0x0015072E
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x06002403 RID: 9219 RVA: 0x0015234C File Offset: 0x0015074C
	Private Iterator Function loop_cr() As IEnumerator
		AudioManager.Play("level_mermaid_turtle_cannon")
		Dim t As Single = 0F
		Dim bulletTime As Single = Me.properties.bulletTimeToExplode.RandomFloat()
		Dim targetDistance As Single = Me.properties.bulletSpeed * bulletTime
		Dim apex As Single = targetDistance + 50F
		Dim launchSpeed As Single = Mathf.Sqrt(4000F * apex)
		Dim timeToApex As Single = launchSpeed / 2000F
		Dim launchY As Single = MyBase.transform.position.y
		While t < timeToApex OrElse MyBase.transform.position.y > targetDistance + launchY
			t += CupheadTime.FixedDelta
			Dim y As Single = launchY + launchSpeed * t - 1000F * t * t
			MyBase.transform.SetPosition(Nothing, New Single?(y), Nothing)
			Yield New WaitForFixedUpdate()
		End While
		For Each text As String In Me.explodePattern.Split(New Char() { "-"c })
			Dim num As Single = 0F
			Parser.FloatTryParse(text, num)
			Me.spreadshotPrefab.Create(MyBase.transform.position, num, Me.properties.spreadshotBulletSpeed, Me.properties.spiralRate)
		Next
		Me.explodeEffectPrefab.Create(MyBase.transform.position)
		Me.Die()
		Return
	End Function

	' Token: 0x06002404 RID: 9220 RVA: 0x00152367 File Offset: 0x00150767
	Protected Overrides Sub Die()
		MyBase.Die()
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
	End Sub

	' Token: 0x04002CD0 RID: 11472
	<SerializeField()>
	Private spreadshotPrefab As FlyingMermaidLevelTurtleSpiralProjectile

	' Token: 0x04002CD1 RID: 11473
	<SerializeField()>
	Private explodeEffectPrefab As Effect

	' Token: 0x04002CD2 RID: 11474
	Private explodePattern As String

	' Token: 0x04002CD3 RID: 11475
	Private properties As LevelProperties.FlyingMermaid.Turtle
End Class
