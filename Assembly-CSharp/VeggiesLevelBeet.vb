Imports System
Imports System.Collections
Imports System.Diagnostics
Imports UnityEngine

' Token: 0x0200083A RID: 2106
Public Class VeggiesLevelBeet
	Inherits LevelProperties.Veggies.Entity

	' Token: 0x17000422 RID: 1058
	' (get) Token: 0x060030C9 RID: 12489 RVA: 0x001CB398 File Offset: 0x001C9798
	' (set) Token: 0x060030CA RID: 12490 RVA: 0x001CB3A0 File Offset: 0x001C97A0
	Public Property state As VeggiesLevelBeet.State

	' Token: 0x1400005A RID: 90
	' (add) Token: 0x060030CB RID: 12491 RVA: 0x001CB3AC File Offset: 0x001C97AC
	' (remove) Token: 0x060030CC RID: 12492 RVA: 0x001CB3E4 File Offset: 0x001C97E4
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnDamageTakenEvent As VeggiesLevelBeet.OnDamageTakenHandler

	' Token: 0x060030CD RID: 12493 RVA: 0x001CB41A File Offset: 0x001C981A
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.CreatePoints()
	End Sub

	' Token: 0x060030CE RID: 12494 RVA: 0x001CB428 File Offset: 0x001C9828
	Private Sub Start()
		Me.boxCollider = MyBase.GetComponent(Of BoxCollider2D)()
		Me.boxCollider.enabled = False
	End Sub

	' Token: 0x060030CF RID: 12495 RVA: 0x001CB444 File Offset: 0x001C9844
	Protected Overrides Sub OnDrawGizmos()
		MyBase.OnDrawGizmos()
		For Each vector As Vector2 In Me.GetPoints()
			Gizmos.color = New Color(1F, 0F, 0F, 0.5F)
			Gizmos.DrawLine(Me.babyRoot.position, vector)
			Gizmos.color = Color.green
			Gizmos.DrawSphere(vector, 10F)
		Next
		Gizmos.color = Color.red
		Gizmos.DrawLine(Me.babyRoot.position, New Vector3(-150F, 360F, 0F))
		Gizmos.DrawLine(Me.babyRoot.position, New Vector3(640F, 360F, 0F))
	End Sub

	' Token: 0x060030D0 RID: 12496 RVA: 0x001CB520 File Offset: 0x001C9920
	Public Overrides Sub LevelInitWithGroup(propertyGroup As AbstractLevelPropertyGroup)
		MyBase.LevelInitWithGroup(propertyGroup)
		Me.properties = TryCast(propertyGroup, LevelProperties.Veggies.Beet)
		Me.hp = CSng(Me.properties.hp)
		AddHandler MyBase.GetComponent(Of DamageReceiver)().OnDamageTaken, AddressOf Me.OnDamageTaken
		Me.damageDealer = New DamageDealer(1F, 0.2F, True, False, False)
		Me.damageDealer.SetDirection(DamageDealer.Direction.Neutral, MyBase.transform)
	End Sub

	' Token: 0x060030D1 RID: 12497 RVA: 0x001CB594 File Offset: 0x001C9994
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		If Me.state = VeggiesLevelBeet.State.Start Then
			Return
		End If
		If Me.OnDamageTakenEvent IsNot Nothing Then
			Me.OnDamageTakenEvent(info.damage)
		End If
		Me.hp -= info.damage
		If Me.hp <= 0F Then
			Me.Die()
		End If
	End Sub

	' Token: 0x060030D2 RID: 12498 RVA: 0x001CB5F2 File Offset: 0x001C99F2
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x060030D3 RID: 12499 RVA: 0x001CB610 File Offset: 0x001C9A10
	Private Sub OnInAnimComplete()
		Me.boxCollider.enabled = True
		Me.state = VeggiesLevelBeet.State.Go
		MyBase.StartCoroutine(Me.beet_cr())
	End Sub

	' Token: 0x060030D4 RID: 12500 RVA: 0x001CB632 File Offset: 0x001C9A32
	Private Sub OnDeathAnimComplete()
		Me.state = VeggiesLevelBeet.State.Complete
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
	End Sub

	' Token: 0x060030D5 RID: 12501 RVA: 0x001CB646 File Offset: 0x001C9A46
	Private Sub Die()
		Me.StopAllCoroutines()
		MyBase.StartCoroutine(Me.die_cr())
	End Sub

	' Token: 0x060030D6 RID: 12502 RVA: 0x001CB65C File Offset: 0x001C9A5C
	Private Function GetPoints() As Vector2()
		Dim array As Vector2() = New Vector2(7) {}
		For i As Integer = 0 To 8 - 1
			Dim num As Single = CSng(i) / 7F
			array(i) = Vector2.Lerp(New Vector2(-150F, 360F), New Vector2(640F, 360F), num)
		Next
		Return array
	End Function

	' Token: 0x060030D7 RID: 12503 RVA: 0x001CB6BC File Offset: 0x001C9ABC
	Private Sub CreatePoints()
		Dim array As Vector2() = Me.GetPoints()
		Me.points = New Transform(array.Length - 1) {}
		For i As Integer = 0 To Me.points.Length - 1
			Me.points(i) = New GameObject("Point " + i).transform
			Me.points(i).position = array(i)
			Me.points(i).SetParent(MyBase.transform)
		Next
	End Sub

	' Token: 0x060030D8 RID: 12504 RVA: 0x001CB74C File Offset: 0x001C9B4C
	Private Function GetPointAngle(i As Integer) As Single
		Me.babyRoot.LookAt2D(Me.points(i))
		Return Me.babyRoot.eulerAngles.z
	End Function

	' Token: 0x060030D9 RID: 12505 RVA: 0x001CB780 File Offset: 0x001C9B80
	Private Iterator Function beet_cr() As IEnumerator
		Dim array As String() = Me.properties.babyPatterns(Global.UnityEngine.Random.Range(0, Me.properties.babyPatterns.Length)).Split(New Char() { ","c })
		Dim numbers As Integer() = New Integer(array.Length - 1) {}
		For j As Integer = 0 To numbers.Length - 1
			If Not Parser.IntTryParse(array(j), numbers(j)) Then
				Global.Debug.LogError("Veggies.Beet.babyPatterns is not formatted correctly!" & vbLf & "Expecting 4,5,6,5,4", Nothing)
				Me.StopAllCoroutines()
			End If
		Next
		Dim typeIndex As Integer = 0
		Dim specialIndex As Integer = Me.properties.alternateRate.RandomInt()
		Dim type As VeggiesLevelBeetBaby.Type = VeggiesLevelBeetBaby.Type.Regular
		While True
			Yield CupheadTime.WaitForSeconds(Me, Me.properties.idleTime)
			MyBase.animator.SetTrigger("Shoot_Start")
			Yield CupheadTime.WaitForSeconds(Me, 1F)
			Dim [loop] As Integer = 0
			Dim point As Integer = 0
			While [loop] < numbers.Length
				For i As Integer = 0 To numbers([loop]) - 1
					Dim newPoint As Integer
					newPoint = point
					While newPoint = point
						newPoint = Global.UnityEngine.Random.Range(0, 8)
					End While
					point = newPoint
					If typeIndex >= specialIndex Then
						type = If((Not Rand.Bool()), VeggiesLevelBeetBaby.Type.Fat, VeggiesLevelBeetBaby.Type.Pink)
						typeIndex = 0
						specialIndex = Me.properties.alternateRate.RandomInt()
					Else
						type = VeggiesLevelBeetBaby.Type.Regular
					End If
					typeIndex += 1
					MyBase.animator.SetTrigger("Shoot_" + type.ToString())
					Me.babyPrefab.Create(type, Me.properties.babySpeedUp, CSng(Me.properties.babySpeedSpread), Me.properties.babySpreadAngle, Me.babyRoot.position, Me.GetPointAngle(point))
					Yield CupheadTime.WaitForSeconds(Me, Me.properties.babyDelay)
				Next
				[loop] += 1
				If [loop] < numbers.Length Then
					Yield CupheadTime.WaitForSeconds(Me, Me.properties.babyGroupDelay)
				End If
			End While
			MyBase.animator.SetTrigger("Shoot_End")
			Yield CupheadTime.WaitForSeconds(Me, Me.properties.babyGroupDelay)
		End While
		Return
	End Function

	' Token: 0x060030DA RID: 12506 RVA: 0x001CB79C File Offset: 0x001C9B9C
	Private Iterator Function die_cr() As IEnumerator
		Me.boxCollider.enabled = False
		MyBase.animator.SetTrigger("Idle")
		Yield MyBase.StartCoroutine(MyBase.dieFlash_cr())
		MyBase.animator.SetTrigger("Dead")
		Return
	End Function

	' Token: 0x04003970 RID: 14704
	Public Const MAX_Y As Single = 360F

	' Token: 0x04003971 RID: 14705
	Private Const POINTS_X_MIN As Single = -150F

	' Token: 0x04003972 RID: 14706
	Private Const POINTS_X_MAX As Single = 640F

	' Token: 0x04003973 RID: 14707
	Private Const POINTS_COUNT As Integer = 8

	' Token: 0x04003975 RID: 14709
	<SerializeField()>
	Private babyRoot As Transform

	' Token: 0x04003976 RID: 14710
	<SerializeField()>
	Private babyPrefab As VeggiesLevelBeetBaby

	' Token: 0x04003977 RID: 14711
	Private properties As LevelProperties.Veggies.Beet

	' Token: 0x04003978 RID: 14712
	Private boxCollider As BoxCollider2D

	' Token: 0x04003979 RID: 14713
	Private hp As Single

	' Token: 0x0400397A RID: 14714
	Private points As Transform()

	' Token: 0x0400397B RID: 14715
	Private damageDealer As DamageDealer

	' Token: 0x0200083B RID: 2107
	Public Enum State
		' Token: 0x0400397E RID: 14718
		Start
		' Token: 0x0400397F RID: 14719
		Go
		' Token: 0x04003980 RID: 14720
		Complete
	End Enum

	' Token: 0x0200083C RID: 2108
	' (Invoke) Token: 0x060030DC RID: 12508
	Public Delegate Sub OnDamageTakenHandler(damage As Single)
End Class
