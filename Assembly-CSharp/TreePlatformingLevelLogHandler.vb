Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports UnityEngine

' Token: 0x02000892 RID: 2194
Public Class TreePlatformingLevelLogHandler
	Inherits AbstractPausableComponent

	' Token: 0x06003307 RID: 13063 RVA: 0x001DA38C File Offset: 0x001D878C
	Private Sub Start()
		Me.dropPosition = MyBase.transform.position
		Me.SetupLogs()
		Me.HP = Me.maxHP
		Me.damageReceiver = MyBase.GetComponent(Of DamageReceiver)()
		AddHandler Me.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
	End Sub

	' Token: 0x06003308 RID: 13064 RVA: 0x001DA3E0 File Offset: 0x001D87E0
	Private Sub SetupLogs()
		Dim num As Integer = 0
		Me.logs = New List(Of TreePlatformingLevelLog)()
		Me.checkedLogs = New List(Of Boolean)(Me.logOrder.Length)
		MyBase.GetComponent(Of HitFlash)().otherRenderers = New SpriteRenderer(Me.logOrder.Length - 1) {}
		For i As Integer = 0 To Me.logOrder.Length - 1
			Dim treePlatformingLevelLog As TreePlatformingLevelLog = Global.UnityEngine.[Object].Instantiate(Of TreePlatformingLevelLog)(Me.logPrefabs(CInt(Me.logOrder(i))))
			treePlatformingLevelLog.transform.position = New Vector3(MyBase.transform.position.x, CSng(Level.Current.Ceiling) + 300F)
			treePlatformingLevelLog.transform.parent = MyBase.transform
			treePlatformingLevelLog.animator.SetBool("hasLegs", i = 0)
			Dim component As Renderer = treePlatformingLevelLog.GetComponent(Of SpriteRenderer)()
			Dim num2 As Integer = num
			num = num2 + 1
			component.sortingOrder = num2
			treePlatformingLevelLog.GetComponent(Of DamageReceiver)().enabled = False
			treePlatformingLevelLog.SetDirection(Me.facingRight)
			Me.logs.Add(treePlatformingLevelLog)
			Me.checkedLogs.Add(False)
			MyBase.GetComponent(Of HitFlash)().otherRenderers(i) = treePlatformingLevelLog.GetComponent(Of SpriteRenderer)()
			If treePlatformingLevelLog.CanShoot Then
				Me.shootableLogs += 1
			End If
		Next
		Me.amountToKillLog = Me.maxHP / CSng(Me.logs.Count)
		MyBase.StartCoroutine(Me.check_to_start_cr())
	End Sub

	' Token: 0x06003309 RID: 13065 RVA: 0x001DA54C File Offset: 0x001D894C
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		Me.HP -= info.damage
		If Me.HP < Me.amountToKillLog * CSng(Me.logs.Count) Then
			If Me.HP > 0F Then
				Me.KillLog()
			Else
				If Me.logs.Count > 0 AndAlso Me.logs(0) IsNot Nothing Then
					Me.logs(0).KillLog()
				End If
				Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
			End If
		End If
	End Sub

	' Token: 0x0600330A RID: 13066 RVA: 0x001DA5EC File Offset: 0x001D89EC
	Private Sub KillLog()
		If Me.logs.Count - 1 > 0 Then
			Me.logs(Me.logs.Count - 1).KillLog()
			Me.logs.RemoveAt(Me.logs.Count - 1)
			If Me.checkedLogs.Count - 1 > 0 Then
				Me.checkedLogs.RemoveAt(Me.logs.Count - 1)
			End If
		End If
		If Me.logs.Count > 0 Then
			MyBase.GetComponent(Of BoxCollider2D)().offset = New Vector2(0F, (Me.logs(0).transform.localPosition.y + Me.logs(Me.logs.Count - 1).transform.localPosition.y) / 2F)
			MyBase.GetComponent(Of BoxCollider2D)().size = New Vector2(Me.logs(0).GetComponent(Of BoxCollider2D)().bounds.size.x, Me.logs(0).GetComponent(Of BoxCollider2D)().bounds.size.y * CSng(Me.logs.Count))
		End If
	End Sub

	' Token: 0x0600330B RID: 13067 RVA: 0x001DA74C File Offset: 0x001D8B4C
	Private Iterator Function check_to_start_cr() As IEnumerator
		MyBase.StartCoroutine(Me.drop_logs_cr())
		Yield Nothing
		Return
	End Function

	' Token: 0x0600330C RID: 13068 RVA: 0x001DA768 File Offset: 0x001D8B68
	Private Iterator Function drop_logs_cr() As IEnumerator
		Dim t As Single = 0F
		Dim time As Single = 0.4F
		Dim wait As YieldInstruction = New WaitForFixedUpdate()
		For i As Integer = 0 To Me.logs.Count - 1
			Dim offset As Single = If((i <> 0), (Me.logs(i).GetComponent(Of BoxCollider2D)().bounds.size.y / 2F + Me.logs(i - 1).GetComponent(Of BoxCollider2D)().bounds.size.y / 2F), 0F)
			Dim start As Single = CupheadLevelCamera.Current.Bounds.yMax + 300F
			Dim [end] As Single = Me.dropPosition.y + offset
			While t < time
				If Me.logs(i) IsNot Nothing Then
					t += CupheadTime.FixedDelta
					Dim num As Single = EaseUtils.Ease(EaseUtils.EaseType.punch, 0F, 1F, t / time)
					Me.logs(i).transform.SetPosition(Nothing, New Single?(Mathf.Lerp(start, [end], num)), Nothing)
				End If
				Yield wait
			End While
			Me.effect.Create(New Vector3(Me.logs(i).transform.position.x, Me.logs(i).transform.position.y - Me.logs(i).GetComponent(Of BoxCollider2D)().bounds.size.y / 2F))
			t = 0F
			Me.dropPosition = Me.logs(i).transform.position
			Me.logs(i).start = Me.logs(i).transform.position.y
			Yield wait
		Next
		For Each treePlatformingLevelLog As TreePlatformingLevelLog In Me.logs
			treePlatformingLevelLog.GetComponent(Of DamageReceiver)().enabled = True
		Next
		MyBase.GetComponent(Of BoxCollider2D)().offset = New Vector2(0F, (Me.logs(0).transform.localPosition.y + Me.logs(Me.logs.Count - 1).transform.localPosition.y) / 2F)
		MyBase.GetComponent(Of BoxCollider2D)().size = New Vector2(Me.logs(0).GetComponent(Of BoxCollider2D)().bounds.size.x, Me.logs(0).GetComponent(Of BoxCollider2D)().bounds.size.y * CSng(Me.logs.Count))
		MyBase.StartCoroutine(Me.shoot_cr())
		Return
	End Function

	' Token: 0x0600330D RID: 13069 RVA: 0x001DA784 File Offset: 0x001D8B84
	Private Iterator Function check_to_slide_cr() As IEnumerator
		Dim amount As Single = 0F
		Dim indexToSlide As Integer = 1000
		While True
			Dim hasRemoved As Boolean = False
			Dim i As Integer = 0
			While i < Me.logs.Count
				If Me.logs(i).isDying AndAlso Not hasRemoved Then
					amount = Me.logs(i).GetComponent(Of BoxCollider2D)().bounds.size.y
					If Me.logs(i).CanShoot Then
						Me.shootableLogs -= 1
					End If
					Me.logs.RemoveAt(i)
					Me.checkedLogs.RemoveAt(i)
					indexToSlide = i
					hasRemoved = True
				Else
					If i >= indexToSlide Then
						While Me.logs(i).isSliding
							Yield Nothing
						End While
						If Me.logs(i) IsNot Nothing Then
							Me.logs(i).SlideDown(amount)
						End If
					End If
					If i = Me.logs.Count - 1 Then
						indexToSlide = 1000
					End If
					i += 1
				End If
			End While
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x0600330E RID: 13070 RVA: 0x001DA7A0 File Offset: 0x001D8BA0
	Private Iterator Function shoot_cr() As IEnumerator
		Dim rand As Integer = 0
		While Me.shootableLogs > 0 AndAlso Me.logs.Count > 0
			Yield CupheadTime.WaitForSeconds(Me, Me.logs(rand).ShootDelay)
			For i As Integer = 0 To Me.checkedLogs.Count - 1
				Me.checkedLogs(i) = False
			Next
			While Me.checkedLogs.Contains(False)
				rand = Global.UnityEngine.Random.Range(0, Me.checkedLogs.Count)
				If Me.logs(rand).CanShoot AndAlso Not Me.checkedLogs(rand) Then
					AudioManager.Play("level_platform_logface_attack")
					Me.emitAudioFromObject.Add("level_platform_logface_attack")
					Me.logs(rand).OnShoot()
					Exit While
				End If
				Me.checkedLogs(rand) = True
			End While
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x0600330F RID: 13071 RVA: 0x001DA7BC File Offset: 0x001D8BBC
	Protected Overrides Sub OnDrawGizmos()
		MyBase.OnDrawGizmos()
		Dim num As Single = 1000F
		Gizmos.color = Color.green
		Gizmos.DrawWireCube(New Vector3(MyBase.transform.position.x, MyBase.transform.position.y + num / 2F), New Vector3(Me.logPrefabs(0).GetComponent(Of SpriteRenderer)().bounds.size.x, num, 0F))
	End Sub

	' Token: 0x04003B2D RID: 15149
	<SerializeField()>
	Private maxHP As Single

	' Token: 0x04003B2E RID: 15150
	Private HP As Single

	' Token: 0x04003B2F RID: 15151
	Private amountToKillLog As Single

	' Token: 0x04003B30 RID: 15152
	<SerializeField()>
	Private facingRight As Boolean

	' Token: 0x04003B31 RID: 15153
	<Header("SET UP LOGS HERE:")>
	<SerializeField()>
	Private logOrder As TreePlatformingLevelLogHandler.LogTypes()

	' Token: 0x04003B32 RID: 15154
	<Header("DON'T TOUCH THIS:")>
	<SerializeField()>
	Private logPrefabs As TreePlatformingLevelLog()

	' Token: 0x04003B33 RID: 15155
	<SerializeField()>
	Private effect As Effect

	' Token: 0x04003B34 RID: 15156
	Private logs As List(Of TreePlatformingLevelLog)

	' Token: 0x04003B35 RID: 15157
	Private dropPosition As Vector3

	' Token: 0x04003B36 RID: 15158
	Private shootableLogs As Integer

	' Token: 0x04003B37 RID: 15159
	Private logsKilled As Integer

	' Token: 0x04003B38 RID: 15160
	Private checkedLogs As List(Of Boolean)

	' Token: 0x04003B39 RID: 15161
	Private damageDealer As DamageDealer

	' Token: 0x04003B3A RID: 15162
	Private damageReceiver As DamageReceiver

	' Token: 0x02000893 RID: 2195
	Private Enum LogTypes
		' Token: 0x04003B3C RID: 15164
		A
		' Token: 0x04003B3D RID: 15165
		B
		' Token: 0x04003B3E RID: 15166
		C
		' Token: 0x04003B3F RID: 15167
		D
		' Token: 0x04003B40 RID: 15168
		E
		' Token: 0x04003B41 RID: 15169
		F
	End Enum
End Class
