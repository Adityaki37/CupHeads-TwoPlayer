Imports System
Imports System.Collections
Imports System.Diagnostics
Imports UnityEngine

' Token: 0x02000772 RID: 1906
Public Class RobotLevelRobot
	Inherits LevelProperties.Robot.Entity

	' Token: 0x14000046 RID: 70
	' (add) Token: 0x06002985 RID: 10629 RVA: 0x00184004 File Offset: 0x00182404
	' (remove) Token: 0x06002986 RID: 10630 RVA: 0x0018403C File Offset: 0x0018243C
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnDeathEvent As Action

	' Token: 0x14000047 RID: 71
	' (add) Token: 0x06002987 RID: 10631 RVA: 0x00184074 File Offset: 0x00182474
	' (remove) Token: 0x06002988 RID: 10632 RVA: 0x001840AC File Offset: 0x001824AC
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnPrimaryDeathEvent As Action

	' Token: 0x14000048 RID: 72
	' (add) Token: 0x06002989 RID: 10633 RVA: 0x001840E4 File Offset: 0x001824E4
	' (remove) Token: 0x0600298A RID: 10634 RVA: 0x0018411C File Offset: 0x0018251C
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnSecondaryDeathEvent As Action

	' Token: 0x14000049 RID: 73
	' (add) Token: 0x0600298B RID: 10635 RVA: 0x00184154 File Offset: 0x00182554
	' (remove) Token: 0x0600298C RID: 10636 RVA: 0x0018418C File Offset: 0x0018258C
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Private Event callback As Action

	' Token: 0x0600298D RID: 10637 RVA: 0x001841C4 File Offset: 0x001825C4
	Protected Overrides Sub Awake()
		For Each collisionChild As CollisionChild In Me.collisionChilds
			MyBase.RegisterCollisionChild(collisionChild)
		Next
		MyBase.Awake()
	End Sub

	' Token: 0x0600298E RID: 10638 RVA: 0x00184200 File Offset: 0x00182600
	Public Overrides Sub LevelInit(properties As LevelProperties.Robot)
		AddHandler Level.Current.OnIntroEvent, AddressOf Me.OnIntro
		If Level.Current.mode = Level.Mode.Easy Then
			AddHandler Level.Current.OnWinEvent, AddressOf Me.OnDeathDance
		End If
		Me.damageDealer = DamageDealer.NewEnemy()
		Dim num As Single = 0F
		Dim num2 As Single = num
		Me.walkTime = num
		Me.walkPCT = num2
		MyBase.StartCoroutine(Me.disableIntro_cr())
		MyBase.LevelInit(properties)
	End Sub

	' Token: 0x0600298F RID: 10639 RVA: 0x0018427C File Offset: 0x0018267C
	Private Iterator Function disableIntro_cr() As IEnumerator
		Yield New WaitForEndOfFrame()
		MyBase.animator.enabled = False
		Return
	End Function

	' Token: 0x06002990 RID: 10640 RVA: 0x00184298 File Offset: 0x00182698
	Private Sub Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
		If Me.introEnded Then
			Dim num As Single = Mathf.Max(PlayerManager.GetNext().center.x, PlayerManager.GetNext().center.x)
			If num > MyBase.transform.position.x Then
				Me.UpdatePosition(True)
			Else
				Me.UpdatePosition(False)
			End If
		End If
	End Sub

	' Token: 0x06002991 RID: 10641 RVA: 0x0018431C File Offset: 0x0018271C
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
		MyBase.OnCollisionPlayer(hit, phase)
	End Sub

	' Token: 0x06002992 RID: 10642 RVA: 0x0018433C File Offset: 0x0018273C
	Private Sub IntroEnded()
		AudioManager.Play("robot_vocals_laugh")
		Me.emitAudioFromObject.Add("robot_vocals_laugh")
		Me.introEnded = True
		MyBase.animator.SetBool("MainAnimationActive", False)
		Me.head.GetComponent(Of RobotLevelRobotHead)().InitBodyPart(Me, MyBase.properties, 1, 1, 0F)
		Me.chest.GetComponent(Of RobotLevelRobotChest)().InitBodyPart(Me, MyBase.properties, 0, 1, 0F)
		Me.hatch.GetComponent(Of RobotLevelRobotHatch)().InitBodyPart(Me, MyBase.properties, 0, 1, 0F)
	End Sub

	' Token: 0x06002993 RID: 10643 RVA: 0x001843D5 File Offset: 0x001827D5
	Public Sub TriggerPhaseTwo(callback As Action)
		MyBase.animator.Play("Phase2 Transition", 2)
		Me.callback = callback
	End Sub

	' Token: 0x06002994 RID: 10644 RVA: 0x001843EF File Offset: 0x001827EF
	Private Sub OnDeathDance()
		Me.chest.animator.Play("Off", 1)
		MyBase.animator.Play("Death Dance")
		MyBase.StartCoroutine(Me.death_cr())
	End Sub

	' Token: 0x06002995 RID: 10645 RVA: 0x00184424 File Offset: 0x00182824
	Private Iterator Function death_cr() As IEnumerator
		Yield New WaitForEndOfFrame()
		For i As Integer = 0 To 3 - 1
			MyBase.transform.GetChild(i).gameObject.SetActive(False)
		Next
		If Me.OnDeathEvent IsNot Nothing Then
			Me.OnDeathEvent()
		End If
		If Level.Current.mode <> Level.Mode.Easy AndAlso Me.callback IsNot Nothing Then
			Me.callback()
		End If
		Return
	End Function

	' Token: 0x06002996 RID: 10646 RVA: 0x0018443F File Offset: 0x0018283F
	Private Sub OnRobotIntro()
		Me.chest.GetComponent(Of RobotLevelRobotChest)().InitAnims()
		Me.hatch.GetComponent(Of RobotLevelRobotHatch)().InitAnims()
	End Sub

	' Token: 0x06002997 RID: 10647 RVA: 0x00184461 File Offset: 0x00182861
	Private Sub OnIntro()
		Me.SoundRobotIntro()
		MyBase.animator.enabled = True
	End Sub

	' Token: 0x06002998 RID: 10648 RVA: 0x00184478 File Offset: 0x00182878
	Public Sub PrimaryDied()
		If Me.OnPrimaryDeathEvent IsNot Nothing Then
			Me.OnPrimaryDeathEvent()
		End If
		Me.remainingPrimaryAttacks -= 1
		If Me.remainingPrimaryAttacks <= 0 AndAlso Me.OnSecondaryDeathEvent IsNot Nothing Then
			Me.OnSecondaryDeathEvent()
		End If
	End Sub

	' Token: 0x06002999 RID: 10649 RVA: 0x001844CC File Offset: 0x001828CC
	Private Sub UpdatePosition(closeGap As Boolean)
		Dim num As Single = 4F
		Dim levelTime As Single = Level.Current.LevelTime
		If closeGap Then
			Dim vector As Vector3 = Me.walkingPositions(0).position
			Dim vector2 As Vector3 = Me.walkingPositions(1).position
			Me.Move(vector, vector2, num, 1)
		Else
			Dim vector As Vector3 = Me.walkingPositions(1).position
			Dim vector2 As Vector3 = Me.walkingPositions(0).position
			Me.Move(vector, vector2, num, -1)
		End If
	End Sub

	' Token: 0x0600299A RID: 10650 RVA: 0x00184544 File Offset: 0x00182944
	Private Sub Move(startPosition As Vector3, endPosition As Vector3, duration As Single, direction As Integer)
		Me.walkTime += CupheadTime.Delta * CSng(direction)
		If direction < 0 Then
			If Me.walkTime <= 0F Then
				Me.walkTime = 0F
			End If
		ElseIf Me.walkTime >= duration Then
			Me.walkTime = duration
		End If
		Me.walkPCT = Me.walkTime / duration
		If Me.walkPCT >= 1F Then
			Me.walkPCT = 1F
		End If
		If direction < 0 Then
			Me.walkPCT = 1F - Me.walkPCT
		End If
		MyBase.transform.position = startPosition + (endPosition - startPosition) * Me.walkPCT
	End Sub

	' Token: 0x0600299B RID: 10651 RVA: 0x0018460D File Offset: 0x00182A0D
	Private Sub SpawnSmoke()
		Me.headcannonSmoke.Create(Me.head.transform.position)
	End Sub

	' Token: 0x0600299C RID: 10652 RVA: 0x0018462B File Offset: 0x00182A2B
	Private Sub OnDeathSFX()
		AudioManager.Play("robot_vocals_dying")
		Me.emitAudioFromObject.Add("robot_vocals_dying")
	End Sub

	' Token: 0x0600299D RID: 10653 RVA: 0x00184647 File Offset: 0x00182A47
	Private Sub SoundRobotIntro()
		AudioManager.Play("robot_intro")
		Me.emitAudioFromObject.Add("robot_intro")
	End Sub

	' Token: 0x04003281 RID: 12929
	Private attackCallback As Action

	' Token: 0x04003286 RID: 12934
	<SerializeField()>
	Private headcannonSmoke As Effect

	' Token: 0x04003287 RID: 12935
	<SerializeField()>
	Private walkingPositions As Transform()

	' Token: 0x04003288 RID: 12936
	<SerializeField()>
	Private head As RobotLevelRobotHead

	' Token: 0x04003289 RID: 12937
	<SerializeField()>
	Private chest As RobotLevelRobotBodyPart

	' Token: 0x0400328A RID: 12938
	<SerializeField()>
	Private hatch As RobotLevelRobotHatch

	' Token: 0x0400328B RID: 12939
	<Space(10F)>
	<SerializeField()>
	Private finalForm As GameObject

	' Token: 0x0400328C RID: 12940
	Private introEnded As Boolean

	' Token: 0x0400328D RID: 12941
	Private walkPCT As Single

	' Token: 0x0400328E RID: 12942
	Private walkTime As Single

	' Token: 0x0400328F RID: 12943
	Private remainingPrimaryAttacks As Integer = 3

	' Token: 0x04003290 RID: 12944
	Private damageDealer As DamageDealer

	' Token: 0x04003291 RID: 12945
	<SerializeField()>
	Private collisionChilds As CollisionChild()
End Class
