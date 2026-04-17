Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x0200077A RID: 1914
Public Class RobotLevelBlockade
	Inherits AbstractCollidableObject

	' Token: 0x060029ED RID: 10733 RVA: 0x00188704 File Offset: 0x00186B04
	Public Function Create(origin As Vector3, dir As Integer) As RobotLevelBlockade
		Dim gameObject As GameObject = Global.UnityEngine.[Object].Instantiate(Of GameObject)(MyBase.gameObject)
		gameObject.transform.position = origin + Vector3.up * CSng(dir) * 300F
		Me.rootSegment = gameObject.GetComponent(Of RobotLevelBlockade)()
		Return Me.rootSegment
	End Function

	' Token: 0x060029EE RID: 10734 RVA: 0x00188756 File Offset: 0x00186B56
	Public Sub InitBlockade(dir As Integer, xSpeed As Integer, ySpeed As Integer)
		Me.xSpeed = xSpeed
		Me.ySpeed = ySpeed * -dir
		MyBase.StartCoroutine(Me.move_cr())
	End Sub

	' Token: 0x060029EF RID: 10735 RVA: 0x00188776 File Offset: 0x00186B76
	Protected Overrides Sub Awake()
		Me.damageDealer = DamageDealer.NewEnemy()
		MyBase.Awake()
	End Sub

	' Token: 0x060029F0 RID: 10736 RVA: 0x00188789 File Offset: 0x00186B89
	Private Sub Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x060029F1 RID: 10737 RVA: 0x001887A1 File Offset: 0x00186BA1
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
		MyBase.OnCollisionPlayer(hit, phase)
	End Sub

	' Token: 0x060029F2 RID: 10738 RVA: 0x001887C0 File Offset: 0x00186BC0
	Private Iterator Function move_cr() As IEnumerator
		While True
			MyBase.transform.position += (Vector3.left * CSng(Me.xSpeed) + Vector3.up * CSng(Me.ySpeed)) * CupheadTime.Delta
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x040032CF RID: 13007
	Private Const heightOffset As Single = 300F

	' Token: 0x040032D0 RID: 13008
	Private damageDealer As DamageDealer

	' Token: 0x040032D1 RID: 13009
	Private rootSegment As RobotLevelBlockade

	' Token: 0x040032D2 RID: 13010
	Private xSpeed As Integer

	' Token: 0x040032D3 RID: 13011
	Private ySpeed As Integer
End Class
