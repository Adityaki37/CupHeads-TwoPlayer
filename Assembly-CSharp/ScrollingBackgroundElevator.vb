Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000B19 RID: 2841
Public Class ScrollingBackgroundElevator
	Inherits AbstractPausableComponent

	' Token: 0x060044D0 RID: 17616 RVA: 0x00246E00 File Offset: 0x00245200
	Public Sub SetUp(direction As Vector3, speed As Single)
		If Me.isBackground Then
			Me.startPos = Me.firstSprite.transform.position
		Else
			Me.startPos = MyBase.transform.position + direction.normalized * -800F
		End If
		Me.endPos = Me.lastSprite.transform.position
		Me.direction = direction
		Me.speed = speed
		MyBase.StartCoroutine(Me.move_cr())
	End Sub

	' Token: 0x060044D1 RID: 17617 RVA: 0x00246E8C File Offset: 0x0024528C
	Private Iterator Function move_cr() As IEnumerator
		While Not Me.ending
			If Me.isBackground Then
				While MyBase.transform.position <> Me.endPos AndAlso Not Me.ending
					MyBase.transform.position = Vector3.MoveTowards(MyBase.transform.position, Me.endPos, Me.speed * CupheadTime.Delta)
					Yield Nothing
				End While
			Else
				While (MyBase.transform.position.y < Me.endPos.y AndAlso MyBase.transform.position.x > Me.endPos.x AndAlso Not Me.ending) OrElse (Me.isClouds AndAlso Me.ending)
					MyBase.transform.position -= Me.direction * Me.speed * CupheadTime.Delta
					Yield Nothing
				End While
			End If
			If Not Me.easingOut Then
				MyBase.transform.position = Me.startPos
			End If
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x060044D2 RID: 17618 RVA: 0x00246EA7 File Offset: 0x002452A7
	Public Sub EaseoutSpeed(time As Single)
		MyBase.StartCoroutine(Me.ease_speed_cr(time))
		Me.easingOut = True
	End Sub

	' Token: 0x060044D3 RID: 17619 RVA: 0x00246EC0 File Offset: 0x002452C0
	Private Iterator Function ease_speed_cr(time As Single) As IEnumerator
		Dim startSpeed As Single = Me.speed
		Dim t As Single = 0F
		While t < time
			t += CupheadTime.Delta
			Me.speed = Mathf.Lerp(startSpeed, 0F, t / time)
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x060044D4 RID: 17620 RVA: 0x00246EE2 File Offset: 0x002452E2
	Protected Overrides Sub OnDrawGizmos()
		MyBase.OnDrawGizmos()
		Gizmos.color = New Color(0F, 0F, 1F, 1F)
		Gizmos.DrawWireSphere(MyBase.transform.position, 100F)
	End Sub

	' Token: 0x04004A90 RID: 19088
	<SerializeField()>
	Private isClouds As Boolean

	' Token: 0x04004A91 RID: 19089
	<SerializeField()>
	Private isBackground As Boolean

	' Token: 0x04004A92 RID: 19090
	<SerializeField()>
	Private firstSprite As SpriteRenderer

	' Token: 0x04004A93 RID: 19091
	<SerializeField()>
	Private lastSprite As SpriteRenderer

	' Token: 0x04004A94 RID: 19092
	Private speed As Single

	' Token: 0x04004A95 RID: 19093
	Private startPos As Vector3

	' Token: 0x04004A96 RID: 19094
	Private endPos As Vector3

	' Token: 0x04004A97 RID: 19095
	Private direction As Vector3

	' Token: 0x04004A98 RID: 19096
	Public ending As Boolean

	' Token: 0x04004A99 RID: 19097
	Public easingOut As Boolean
End Class
