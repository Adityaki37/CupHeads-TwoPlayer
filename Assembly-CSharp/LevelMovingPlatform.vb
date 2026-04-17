Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x0200043A RID: 1082
Public Class LevelMovingPlatform
	Inherits LevelPlatform

	' Token: 0x1700028C RID: 652
	' (get) Token: 0x06000FE7 RID: 4071 RVA: 0x0009DA28 File Offset: 0x0009BE28
	Public Overridable ReadOnly Property Ease As EaseUtils.EaseType
		Get
			Return EaseUtils.EaseType.linear
		End Get
	End Property

	' Token: 0x06000FE8 RID: 4072 RVA: 0x0009DA2C File Offset: 0x0009BE2C
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.startPos = MyBase.transform.position
		Me.endPos = Me.startPos + Me.[end]
		MyBase.StartCoroutine(Me.move_cr())
	End Sub

	' Token: 0x06000FE9 RID: 4073 RVA: 0x0009DA7C File Offset: 0x0009BE7C
	Private Iterator Function move_cr() As IEnumerator
		While True
			Yield MyBase.StartCoroutine(Me.goTo_cr(Me.startPos, Me.endPos))
			Yield New WaitForSeconds(1F)
			Yield MyBase.StartCoroutine(Me.goTo_cr(Me.endPos, Me.startPos))
			Yield New WaitForSeconds(1F)
		End While
		Return
	End Function

	' Token: 0x06000FEA RID: 4074 RVA: 0x0009DA98 File Offset: 0x0009BE98
	Private Iterator Function goTo_cr(start As Vector3, [end] As Vector3) As IEnumerator
		Dim t As Single = 0F
		MyBase.transform.position = start
		While t < Me.time
			Dim val As Single = t / Me.time
			Dim pos As Vector3 = MyBase.transform.position
			pos.x = EaseUtils.Ease(Me.Ease, start.x, [end].x, val)
			pos.y = EaseUtils.Ease(Me.Ease, start.y, [end].y, val)
			MyBase.transform.position = pos
			t += Time.deltaTime
			Yield MyBase.StartCoroutine(MyBase.WaitForPause_CR())
		End While
		MyBase.transform.position = [end]
		Return
	End Function

	' Token: 0x06000FEB RID: 4075 RVA: 0x0009DAC4 File Offset: 0x0009BEC4
	Protected Overrides Sub OnDrawGizmos()
		MyBase.OnDrawGizmos()
		Gizmos.color = Color.magenta
		If Application.isPlaying Then
			Gizmos.DrawLine(Me.startPos, Me.endPos)
			Gizmos.DrawWireSphere(MyBase.transform.position, 5F)
		Else
			Gizmos.DrawLine(MyBase.baseTransform.position, MyBase.baseTransform.position + Me.[end])
		End If
	End Sub

	' Token: 0x04001980 RID: 6528
	<SerializeField()>
	Private time As Single

	' Token: 0x04001981 RID: 6529
	<SerializeField()>
	Private [end] As Vector2

	' Token: 0x04001982 RID: 6530
	Private startPos As Vector3

	' Token: 0x04001983 RID: 6531
	Private endPos As Vector3
End Class
