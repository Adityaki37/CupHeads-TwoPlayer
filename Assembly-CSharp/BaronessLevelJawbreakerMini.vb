Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports UnityEngine

' Token: 0x020004F3 RID: 1267
Public Class BaronessLevelJawbreakerMini
	Inherits BaronessLevelMiniBossBase

	' Token: 0x17000323 RID: 803
	' (get) Token: 0x06001632 RID: 5682 RVA: 0x000C741B File Offset: 0x000C581B
	' (set) Token: 0x06001633 RID: 5683 RVA: 0x000C7423 File Offset: 0x000C5823
	Public Property state As BaronessLevelJawbreakerMini.State

	' Token: 0x06001634 RID: 5684 RVA: 0x000C742C File Offset: 0x000C582C
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.damageDealer = DamageDealer.NewEnemy()
		Me.aim = MyBase.transform
		Me.rotateDeath = False
		Me.bigPath = New List(Of Vector3)()
	End Sub

	' Token: 0x06001635 RID: 5685 RVA: 0x000C7460 File Offset: 0x000C5860
	Protected Overrides Sub Start()
		MyBase.Start()
		Me.layerSwitch = 3
		Me.fadeTime = 2F
		Me.sprite.GetComponent(Of SpriteRenderer)().sortingLayerName = SpriteLayer.Background.ToString()
		Me.sprite.GetComponent(Of SpriteRenderer)().sortingOrder = 150
		MyBase.StartCoroutine(Me.check_rotation_cr())
		MyBase.StartCoroutine(Me.switch_cr())
	End Sub

	' Token: 0x06001636 RID: 5686 RVA: 0x000C74D3 File Offset: 0x000C58D3
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x06001637 RID: 5687 RVA: 0x000C74F4 File Offset: 0x000C58F4
	Public Sub Init(properties As LevelProperties.Baroness.Jawbreaker, pos As Vector2, targetPos As Transform, rotationSpeed As Single)
		Me.properties = properties
		Me.rotationSpeed = rotationSpeed
		MyBase.transform.position = pos
		Me.targetPosition = targetPos
		Me.state = BaronessLevelJawbreakerMini.State.Spawned
		MyBase.StartCoroutine(Me.blink_cr())
		MyBase.StartCoroutine(Me.calculate_path_cr())
		MyBase.StartCoroutine(Me.move_cr())
	End Sub

	' Token: 0x06001638 RID: 5688 RVA: 0x000C7556 File Offset: 0x000C5956
	Private Sub Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x06001639 RID: 5689 RVA: 0x000C7570 File Offset: 0x000C5970
	Private Sub FixedUpdate()
		If Me.state = BaronessLevelJawbreakerMini.State.Spawned Then
			If Me.bigPath.Count <> 0 Then
				Dim num As Single = Me.pathLength / Me.properties.jawbreakerMiniSpace
				MyBase.transform.position -= MyBase.transform.right * (num * Me.properties.jawbreakerHomingSpeed) * CupheadTime.FixedDelta
				Me.pathLength = 0F
				Me.aim.LookAt2D(2F * MyBase.transform.position - Me.bigPath(0))
				MyBase.transform.rotation = Quaternion.Slerp(MyBase.transform.rotation, Me.aim.rotation, Me.rotationSpeed * CupheadTime.FixedDelta)
				Me.sprite.transform.SetEulerAngles(New Single?(0F), New Single?(0F), New Single?(0F))
				Dim num2 As Single = Vector3.Distance(MyBase.transform.position, Me.bigPath(0))
				If num2 < Me.properties.jawbreakerHomingSpeed / 4F Then
					Me.bigPath.Remove(Me.bigPath(0))
				End If
			End If
			If Me.state = BaronessLevelJawbreakerMini.State.Dying AndAlso Me.rotateDeath Then
				Me.RotateExplode()
				Me.rotateDeath = False
			End If
		End If
	End Sub

	' Token: 0x0600163A RID: 5690 RVA: 0x000C76F8 File Offset: 0x000C5AF8
	Private Iterator Function switch_cr() As IEnumerator
		MyBase.StartCoroutine(Me.fade_color_cr())
		Yield CupheadTime.WaitForSeconds(Me, 3F)
		Me.sprite.GetComponent(Of SpriteRenderer)().sortingLayerName = SpriteLayer.Enemies.ToString()
		Me.sprite.GetComponent(Of SpriteRenderer)().sortingOrder = 250
		Return
	End Function

	' Token: 0x0600163B RID: 5691 RVA: 0x000C7714 File Offset: 0x000C5B14
	Private Sub Turn()
		Me.sprite.transform.SetScale(New Single?(-Me.sprite.transform.localScale.x), New Single?(1F), New Single?(1F))
	End Sub

	' Token: 0x0600163C RID: 5692 RVA: 0x000C7764 File Offset: 0x000C5B64
	Private Iterator Function check_rotation_cr() As IEnumerator
		While True
			If((Me.targetPosition.transform.position.x < MyBase.transform.position.x AndAlso Not Me.lookingLeft) OrElse (Me.targetPosition.transform.position.x > MyBase.transform.position.x AndAlso Me.lookingLeft)) AndAlso Not Me.isTurning Then
				Me.isTurning = True
				MyBase.animator.SetTrigger("Turn")
				Yield MyBase.animator.WaitForAnimationToEnd(Me, "Turn", False, True)
				Me.lookingLeft = Not Me.lookingLeft
				Me.isTurning = False
			End If
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x0600163D RID: 5693 RVA: 0x000C7780 File Offset: 0x000C5B80
	Private Iterator Function move_cr() As IEnumerator
		While True
			Me.pathLength = 0F
			For i As Integer = 0 To Me.bigPath.Count - 1 - 1
				Me.pathLength += Vector3.Distance(Me.bigPath(i), Me.bigPath(i + 1))
				If CupheadTime.Delta Is 0F Then
					Yield Nothing
				End If
			Next
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x0600163E RID: 5694 RVA: 0x000C779C File Offset: 0x000C5B9C
	Private Iterator Function calculate_path_cr() As IEnumerator
		While True
			For i As Integer = 0 To Me.positionsInList - 1
				If Not Mathf.Approximately(CupheadTime.Delta, 0F) Then
					Me.bigPath.Add(Me.targetPosition.transform.position)
				End If
				Yield New WaitForFixedUpdate()
			Next
		End While
		Return
	End Function

	' Token: 0x0600163F RID: 5695 RVA: 0x000C77B7 File Offset: 0x000C5BB7
	Public Sub [Stop]()
		Me.state = BaronessLevelJawbreakerMini.State.Dying
		MyBase.StopCoroutine(Me.blink_cr())
		MyBase.StopCoroutine(Me.calculate_path_cr())
	End Sub

	' Token: 0x06001640 RID: 5696 RVA: 0x000C77D8 File Offset: 0x000C5BD8
	Public Sub StartDying()
		MyBase.StartCoroutine(Me.dying_cr())
	End Sub

	' Token: 0x06001641 RID: 5697 RVA: 0x000C77E8 File Offset: 0x000C5BE8
	Private Sub RotateExplode()
		Dim num As Single = CSng(Global.UnityEngine.Random.Range(0, 360))
		MyBase.transform.rotation = Quaternion.Euler(0F, 0F, num)
	End Sub

	' Token: 0x06001642 RID: 5698 RVA: 0x000C7820 File Offset: 0x000C5C20
	Private Iterator Function dying_cr() As IEnumerator
		Dim collider As Collider2D = MyBase.GetComponent(Of Collider2D)()
		collider.enabled = False
		Yield CupheadTime.WaitForSeconds(Me, 1F)
		MyBase.animator.SetTrigger("Dead")
		Yield CupheadTime.WaitForSeconds(Me, 0.5F)
		Me.rotateDeath = True
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Death_End", False, True)
		Me.KillMini()
		Return
	End Function

	' Token: 0x06001643 RID: 5699 RVA: 0x000C783C File Offset: 0x000C5C3C
	Private Iterator Function blink_cr() As IEnumerator
		While Me.state = BaronessLevelJawbreakerMini.State.Spawned
			MyBase.animator.SetTrigger("Blink")
			Dim timeBetweenNext As Integer = Global.UnityEngine.Random.Range(2, 4)
			Yield CupheadTime.WaitForSeconds(Me, CSng(timeBetweenNext))
		End While
		Return
	End Function

	' Token: 0x06001644 RID: 5700 RVA: 0x000C7858 File Offset: 0x000C5C58
	Private Sub KillMini()
		Me.state = BaronessLevelJawbreakerMini.State.Unspawned
		Dim component As Collider2D = MyBase.GetComponent(Of Collider2D)()
		component.enabled = False
		Me.StopAllCoroutines()
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
	End Sub

	' Token: 0x04001F7E RID: 8062
	Private Const POSITION_FRAME_TIME As Single = 0.083333336F

	' Token: 0x04001F7F RID: 8063
	<SerializeField()>
	Private sprite As Transform

	' Token: 0x04001F81 RID: 8065
	Private rotationSpeed As Single

	' Token: 0x04001F82 RID: 8066
	Private pathLength As Single

	' Token: 0x04001F83 RID: 8067
	Private positionsInList As Integer = 12

	' Token: 0x04001F84 RID: 8068
	Private rotateDeath As Boolean

	' Token: 0x04001F85 RID: 8069
	Private lookingLeft As Boolean = True

	' Token: 0x04001F86 RID: 8070
	Private isTurning As Boolean

	' Token: 0x04001F87 RID: 8071
	Private damageDealer As DamageDealer

	' Token: 0x04001F88 RID: 8072
	Private properties As LevelProperties.Baroness.Jawbreaker

	' Token: 0x04001F89 RID: 8073
	Private currentPos As Vector3

	' Token: 0x04001F8A RID: 8074
	Private targetPosition As Transform

	' Token: 0x04001F8B RID: 8075
	Private aim As Transform

	' Token: 0x04001F8C RID: 8076
	Private bigPath As List(Of Vector3)

	' Token: 0x04001F8D RID: 8077
	Private rotate As Quaternion

	' Token: 0x020004F4 RID: 1268
	Public Enum State
		' Token: 0x04001F8F RID: 8079
		Unspawned
		' Token: 0x04001F90 RID: 8080
		Spawned
		' Token: 0x04001F91 RID: 8081
		Dying
	End Enum
End Class
