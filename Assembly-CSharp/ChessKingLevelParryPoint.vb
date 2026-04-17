Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x0200053D RID: 1341
Public Class ChessKingLevelParryPoint
	Inherits ParrySwitch

	' Token: 0x17000335 RID: 821
	' (get) Token: 0x06001871 RID: 6257 RVA: 0x000DD7CC File Offset: 0x000DBBCC
	' (set) Token: 0x06001872 RID: 6258 RVA: 0x000DD7D4 File Offset: 0x000DBBD4
	Public Property GOT_PARRIED As Boolean

	' Token: 0x17000336 RID: 822
	' (get) Token: 0x06001873 RID: 6259 RVA: 0x000DD7DD File Offset: 0x000DBBDD
	' (set) Token: 0x06001874 RID: 6260 RVA: 0x000DD7E5 File Offset: 0x000DBBE5
	Public Property IS_BLUE As Boolean

	' Token: 0x06001875 RID: 6261 RVA: 0x000DD7EE File Offset: 0x000DBBEE
	Protected Overrides Sub Awake()
		Me.GOT_PARRIED = False
		MyBase.GetComponent(Of Collider2D)().enabled = False
		MyBase.GetComponent(Of SpriteRenderer)().color = Color.grey
		MyBase.Awake()
	End Sub

	' Token: 0x06001876 RID: 6262 RVA: 0x000DD819 File Offset: 0x000DBC19
	Public Sub Init(pos As Vector3)
		MyBase.transform.position = pos
		Me.IS_BLUE = False
		Me.GOT_PARRIED = False
	End Sub

	' Token: 0x06001877 RID: 6263 RVA: 0x000DD838 File Offset: 0x000DBC38
	Public Sub Init(pos As Vector3, dir As Vector3, speed As Single, amount As Single)
		MyBase.GetComponent(Of SpriteRenderer)().color = Color.blue
		MyBase.transform.position = pos
		Me.dir = dir
		Me.amount = amount
		Me.speed = speed
		Me.IS_BLUE = True
		Me.GOT_PARRIED = False
	End Sub

	' Token: 0x06001878 RID: 6264 RVA: 0x000DD885 File Offset: 0x000DBC85
	Public Sub Activate()
		MyBase.GetComponent(Of Collider2D)().enabled = True
		MyBase.GetComponent(Of SpriteRenderer)().color = Color.magenta
	End Sub

	' Token: 0x06001879 RID: 6265 RVA: 0x000DD8A3 File Offset: 0x000DBCA3
	Public Overrides Sub OnParryPrePause(player As AbstractPlayerController)
		MyBase.OnParryPrePause(player)
		Me.GOT_PARRIED = True
		MyBase.GetComponent(Of Collider2D)().enabled = False
		MyBase.GetComponent(Of SpriteRenderer)().color = Color.grey
	End Sub

	' Token: 0x0600187A RID: 6266 RVA: 0x000DD8CF File Offset: 0x000DBCCF
	Public Sub MovePoint()
		If Me.IS_BLUE Then
			MyBase.StartCoroutine(Me.move_cr())
		End If
	End Sub

	' Token: 0x0600187B RID: 6267 RVA: 0x000DD8EC File Offset: 0x000DBCEC
	Private Iterator Function move_cr() As IEnumerator
		Dim startPos As Vector3 = MyBase.transform.position
		Dim endPos As Vector3 = Me.GetEndPos()
		Dim wait As YieldInstruction = New WaitForFixedUpdate()
		Dim t As Single = 0F
		Dim time As Single = Vector3.Distance(startPos, endPos) / Me.speed
		While t < time
			t += CupheadTime.FixedDelta
			MyBase.transform.position = Vector3.Lerp(startPos, endPos, t / time)
			Yield wait
		End While
		MyBase.transform.position = endPos
		Yield Nothing
		Return
	End Function

	' Token: 0x0600187C RID: 6268 RVA: 0x000DD908 File Offset: 0x000DBD08
	Private Function GetEndPos() As Vector3
		If Me.dir = Vector3.right Then
			Return New Vector3(MyBase.transform.position.x + Me.amount, MyBase.transform.position.y)
		End If
		If Me.dir = Vector3.left Then
			Return New Vector3(MyBase.transform.position.x - Me.amount, MyBase.transform.position.y)
		End If
		If Me.dir = Vector3.up Then
			Return New Vector3(MyBase.transform.position.x, MyBase.transform.position.y + Me.amount)
		End If
		Return New Vector3(MyBase.transform.position.x, MyBase.transform.position.y - Me.amount)
	End Function

	' Token: 0x040021A0 RID: 8608
	Private dir As Vector3

	' Token: 0x040021A1 RID: 8609
	Private amount As Single

	' Token: 0x040021A2 RID: 8610
	Private speed As Single
End Class
