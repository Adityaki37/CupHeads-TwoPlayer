Imports System
Imports UnityEngine

' Token: 0x02000771 RID: 1905
Public Class RobotLevelHoseLaser
	Inherits AbstractCollidableObject

	' Token: 0x06002980 RID: 10624 RVA: 0x00183F40 File Offset: 0x00182340
	Public Function Create(pos As Vector3, angle As Single, parent As RobotLevelRobotBodyPart) As RobotLevelHoseLaser
		Dim gameObject As GameObject = Global.UnityEngine.[Object].Instantiate(Of GameObject)(MyBase.gameObject)
		gameObject.transform.parent = parent.transform
		gameObject.transform.position = pos
		gameObject.transform.SetEulerAngles(Nothing, Nothing, New Single?(angle))
		MyBase.GetComponent(Of Collider2D)().enabled = False
		Return gameObject.GetComponent(Of RobotLevelHoseLaser)()
	End Function

	' Token: 0x06002981 RID: 10625 RVA: 0x00183FAB File Offset: 0x001823AB
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.damageDealer = DamageDealer.NewEnemy()
	End Sub

	' Token: 0x06002982 RID: 10626 RVA: 0x00183FBE File Offset: 0x001823BE
	Private Sub Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x06002983 RID: 10627 RVA: 0x00183FD6 File Offset: 0x001823D6
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x04003280 RID: 12928
	Private damageDealer As DamageDealer
End Class
