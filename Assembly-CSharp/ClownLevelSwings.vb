Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x0200056D RID: 1389
Public Class ClownLevelSwings
	Inherits AbstractCollidableObject

	' Token: 0x06001A40 RID: 6720 RVA: 0x000F0275 File Offset: 0x000EE675
	Public Sub Init(pos As Vector3, properties As LevelProperties.Clown.Swing, spacing As Single, enterAngle As Single)
		Me.properties = properties
		MyBase.transform.position = pos
		Me.spacing = spacing
		Me.enterAngle = enterAngle
	End Sub

	' Token: 0x06001A41 RID: 6721 RVA: 0x000F029C File Offset: 0x000EE69C
	Private Sub Start()
		Me.sprite = MyBase.GetComponent(Of SpriteRenderer)()
		Me.defaultColor = MyBase.GetComponent(Of SpriteRenderer)().color
		If Me.isBackSeat Then
			Me.rod.transform.SetEulerAngles(Nothing, Nothing, New Single?(Me.startAngleBack - Me.enterAngle))
		Else
			Me.rod.transform.SetEulerAngles(Nothing, Nothing, New Single?(Me.startAngleFront + Me.enterAngle))
		End If
		MyBase.StartCoroutine(Me.rotation_cr())
		MyBase.StartCoroutine(Me.move_swing_y_cr())
		MyBase.StartCoroutine(Me.move_swing_x_cr())
		If Me.properties.swingDropOn Then
			MyBase.StartCoroutine(Me.player_check_cr())
		End If
	End Sub

	' Token: 0x06001A42 RID: 6722 RVA: 0x000F0384 File Offset: 0x000EE784
	Private Iterator Function move_swing_y_cr() As IEnumerator
		Dim starting As Boolean = True
		Dim distanceLeft As Single = 0F
		Dim distanceRight As Single = 0F
		While True
			Dim pos As Vector3 = MyBase.transform.position
			Dim speed As Single = If((Not starting), 50F, 150F)
			If Me.isBackSeat Then
				distanceLeft = -Me.distAmount
				distanceRight = Me.distAmount + Me.distAmount / 2F
			Else
				distanceLeft = -Me.distAmount - Me.distAmount / 2F
				distanceRight = Me.distAmount
			End If
			If MyBase.transform.position.x > distanceRight OrElse MyBase.transform.position.x < distanceLeft Then
				If MyBase.transform.position.y <> Me.highPoint Then
					pos.y = Mathf.MoveTowards(MyBase.transform.position.y, Me.highPoint, speed * CupheadTime.Delta)
					MyBase.transform.position = pos
				Else
					starting = False
				End If
			ElseIf MyBase.transform.position.y <> Me.lowestPoint Then
				pos.y = Mathf.MoveTowards(MyBase.transform.position.y, Me.lowestPoint, speed * CupheadTime.Delta)
				MyBase.transform.position = pos
			Else
				starting = False
			End If
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06001A43 RID: 6723 RVA: 0x000F03A0 File Offset: 0x000EE7A0
	Private Iterator Function move_swing_x_cr() As IEnumerator
		ClownLevelSwings.moveSpeed = Me.properties.swingSpeed
		Dim size As Single = MyBase.transform.GetComponent(Of Renderer)().bounds.size.x
		Dim sizeDivided As Single = size / 4F
		While True
			If Me.isBackSeat Then
				Dim [end] As Single = 640F - Me.spacing * 5F
				While MyBase.transform.position.x > [end]
					MyBase.transform.position -= MyBase.transform.right * ClownLevelSwings.moveSpeed * CupheadTime.Delta
					Yield Nothing
				End While
				MyBase.transform.position = New Vector3(640F + Me.spacing, Me.highPoint, 0F)
			Else
				Dim end2 As Single = -640F + (Me.spacing * 5F + size)
				MyBase.transform.GetComponent(Of Collider2D)().enabled = True
				While MyBase.transform.position.x < end2
					If MyBase.transform.position.x > 640F + sizeDivided Then
						MyBase.transform.GetComponent(Of Collider2D)().enabled = False
					End If
					MyBase.transform.position += MyBase.transform.right * ClownLevelSwings.moveSpeed * CupheadTime.Delta
					Yield Nothing
				End While
				Me.resetWarning = True
				Me.SwingReappear()
				MyBase.transform.position = New Vector3(-640F - (Me.spacing - size), Me.highPoint, 0F)
			End If
			MyBase.StopCoroutine(Me.rotation_cr())
			Me.enterAngle = 0F
			If Me.isBackSeat Then
				Me.rod.transform.SetEulerAngles(Nothing, Nothing, New Single?(Me.startAngleBack))
			Else
				Me.rod.transform.SetEulerAngles(Nothing, Nothing, New Single?(Me.startAngleFront))
			End If
			MyBase.StartCoroutine(Me.rotation_cr())
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06001A44 RID: 6724 RVA: 0x000F03BC File Offset: 0x000EE7BC
	Private Iterator Function rotation_cr() As IEnumerator
		Dim t As Single = 0F
		If Me.isBackSeat Then
			While Me.rod.transform.eulerAngles.z > Me.endAngleBack
				If CupheadTime.Delta IsNot 0F Then
					Me.rod.transform.SetEulerAngles(Nothing, Nothing, New Single?(Me.rod.transform.eulerAngles.z - t))
					t += 0.001F
				End If
				Yield Nothing
			End While
		Else
			While Me.rod.transform.eulerAngles.z < Me.endAngleFront
				If CupheadTime.Delta IsNot 0F Then
					Me.rod.transform.SetEulerAngles(Nothing, Nothing, New Single?(Me.rod.transform.eulerAngles.z + t))
					t += 0.001F
				End If
				Yield Nothing
			End While
		End If
		Yield Nothing
		Return
	End Function

	' Token: 0x06001A45 RID: 6725 RVA: 0x000F03D8 File Offset: 0x000EE7D8
	Private Iterator Function player_check_cr() As IEnumerator
		Dim player As AbstractPlayerController = PlayerManager.GetNext()
		While True
			While player Is Nothing OrElse player.transform.parent IsNot MyBase.transform
				player = PlayerManager.GetNext()
				Yield Nothing
			End While
			If Not Me.resetWarning Then
				Me.sprite.color = Color.red
				Yield CupheadTime.WaitForSeconds(Me, Me.properties.swingDropWarningDuration)
				Yield Nothing
				Me.sprite.color = Color.black
				MyBase.transform.GetComponent(Of Collider2D)().enabled = False
				Yield CupheadTime.WaitForSeconds(Me, Me.properties.swingfullDropDuration)
			End If
			Me.SwingReappear()
			Me.resetWarning = False
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06001A46 RID: 6726 RVA: 0x000F03F3 File Offset: 0x000EE7F3
	Private Sub SwingReappear()
		MyBase.transform.GetComponent(Of Collider2D)().enabled = True
		Me.sprite.color = Me.defaultColor
	End Sub

	' Token: 0x0400235B RID: 9051
	Public Shared moveSpeed As Single

	' Token: 0x0400235C RID: 9052
	Private Const FALL_GRAVITY As Single = -100F

	' Token: 0x0400235D RID: 9053
	Public isBackSeat As Boolean

	' Token: 0x0400235E RID: 9054
	<SerializeField()>
	Private rod As Transform

	' Token: 0x0400235F RID: 9055
	Private properties As LevelProperties.Clown.Swing

	' Token: 0x04002360 RID: 9056
	Private sprite As SpriteRenderer

	' Token: 0x04002361 RID: 9057
	Private defaultColor As Color

	' Token: 0x04002362 RID: 9058
	Private resetWarning As Boolean

	' Token: 0x04002363 RID: 9059
	Private spacing As Single

	' Token: 0x04002364 RID: 9060
	Private lowestPoint As Single

	' Token: 0x04002365 RID: 9061
	Private highPoint As Single = 100F

	' Token: 0x04002366 RID: 9062
	Private distAmount As Single = 450F

	' Token: 0x04002367 RID: 9063
	Private startAngleFront As Single = 320F

	' Token: 0x04002368 RID: 9064
	Private startAngleBack As Single = 40F

	' Token: 0x04002369 RID: 9065
	Private endAngleFront As Single = 350F

	' Token: 0x0400236A RID: 9066
	Private endAngleBack As Single = 10F

	' Token: 0x0400236B RID: 9067
	Private enterAngle As Single
End Class
