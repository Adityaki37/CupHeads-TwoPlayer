Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020004B4 RID: 1204
Public Class AirplaneLevelBulldogParachute
	Inherits LevelProperties.Airplane.Entity

	' Token: 0x1700030E RID: 782
	' (get) Token: 0x060013BA RID: 5050 RVA: 0x000AEB31 File Offset: 0x000ACF31
	' (set) Token: 0x060013BB RID: 5051 RVA: 0x000AEB39 File Offset: 0x000ACF39
	Public Property isMoving As Boolean

	' Token: 0x060013BC RID: 5052 RVA: 0x000AEB42 File Offset: 0x000ACF42
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.damageDealer = DamageDealer.NewEnemy()
		Me.pinkString = New PatternString(MyBase.properties.CurrentState.parachute.pinkString, True, True)
	End Sub

	' Token: 0x060013BD RID: 5053 RVA: 0x000AEB77 File Offset: 0x000ACF77
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me.WORKAROUND_NullifyFields()
	End Sub

	' Token: 0x060013BE RID: 5054 RVA: 0x000AEB85 File Offset: 0x000ACF85
	Public Overrides Sub LevelInit(properties As LevelProperties.Airplane)
		MyBase.LevelInit(properties)
	End Sub

	' Token: 0x060013BF RID: 5055 RVA: 0x000AEB8E File Offset: 0x000ACF8E
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
		MyBase.OnCollisionPlayer(hit, phase)
	End Sub

	' Token: 0x060013C0 RID: 5056 RVA: 0x000AEBAC File Offset: 0x000ACFAC
	Private Sub Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x060013C1 RID: 5057 RVA: 0x000AEBC4 File Offset: 0x000ACFC4
	Public Sub StartDescent(pos As Vector2, scale As Single)
		Me.isMoving = True
		MyBase.transform.position = pos
		MyBase.transform.localScale = New Vector3(scale, 1F)
		Me.count = 0
		MyBase.StartCoroutine(Me.move_cr())
	End Sub

	' Token: 0x060013C2 RID: 5058 RVA: 0x000AEC14 File Offset: 0x000AD014
	Private Iterator Function move_cr() As IEnumerator
		MyBase.animator.Play("Drop")
		MyBase.animator.Update(0F)
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Drop", False, True)
		Me.isMoving = False
		Return
	End Function

	' Token: 0x060013C3 RID: 5059 RVA: 0x000AEC30 File Offset: 0x000AD030
	Public Sub EarlyExit()
		Me.StopAllCoroutines()
		MyBase.StartCoroutine(Me.early_exit_cr())
		If MyBase.animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.41379312F Then
			MyBase.transform.position = New Vector3(MyBase.transform.position.x, Me.collider.transform.localPosition.y + 400F)
			MyBase.animator.Play("Drop", 0, 0.87356323F)
			MyBase.animator.Update(0F)
		End If
	End Sub

	' Token: 0x060013C4 RID: 5060 RVA: 0x000AECD8 File Offset: 0x000AD0D8
	Private Iterator Function early_exit_cr() As IEnumerator
		If MyBase.animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.41379312F Then
			MyBase.animator.Play("Drop", 0, 0.87356323F)
		End If
		Yield MyBase.animator.WaitForAnimationToStart(Me, "None", False)
		Me.isMoving = False
		If Me.main.isDead AndAlso Me.mainAnimator Then
			Me.mainAnimator.SetBool("InParachuteATK", False)
		End If
		MyBase.gameObject.SetActive(False)
		Return
	End Function

	' Token: 0x060013C5 RID: 5061 RVA: 0x000AECF3 File Offset: 0x000AD0F3
	Private Sub OnDisable()
		MyBase.GetComponent(Of HitFlash)().StopAllCoroutinesWithoutSettingScale()
		MyBase.GetComponent(Of SpriteRenderer)().color = Color.black
	End Sub

	' Token: 0x060013C6 RID: 5062 RVA: 0x000AED10 File Offset: 0x000AD110
	Private Sub AniEvent_Shoot()
		Dim parachute As LevelProperties.Airplane.Parachute = MyBase.properties.CurrentState.parachute
		Dim vector As Vector3 = New Vector3(If((Me.count <> 1), Me.spawnRoot1.position.x, Me.spawnRoot2.position.x), Me.shotYPos(Me.count))
		Dim num As Single = If((Me.count <> 0), If((Me.count <> 1), parachute.shotCReturnDelay.RandomFloat(), parachute.shotBReturnDelay.RandomFloat()), parachute.shotAReturnDelay.RandomFloat())
		If Me.pinkString.PopLetter() = "P"c Then
			Me.boomerangPink.Create(vector, parachute.speedForward, parachute.easeDistanceForward, parachute.speedReturn, parachute.easeDistanceReturn, num, MyBase.transform.localScale.x > 0F, 1)
		Else
			Me.boomerang.Create(vector, parachute.speedForward, parachute.easeDistanceForward, parachute.speedReturn, parachute.easeDistanceReturn, num, MyBase.transform.localScale.x > 0F, 1)
		End If
		Me.count += 1
		AudioManager.Play("sfx_dlc_dogfight_p1_bulldog_bicepflex")
		Me.emitAudioFromObject.Add("sfx_dlc_dogfight_p1_bulldog_bicepflex")
		AudioManager.Play("sfx_dlc_dogfight_dogflexhugovocal")
		Me.emitAudioFromObject.Add("sfx_dlc_dogfight_dogflexhugovocal")
	End Sub

	' Token: 0x060013C7 RID: 5063 RVA: 0x000AEEA6 File Offset: 0x000AD2A6
	Private Sub AniEvent_SFX_BulldogPlane_ParachuteEnd()
		AudioManager.Play("sfx_DLC_Dogfight_P1_Bulldog_SpringsUp")
	End Sub

	' Token: 0x060013C8 RID: 5064 RVA: 0x000AEEB4 File Offset: 0x000AD2B4
	Private Sub WORKAROUND_NullifyFields()
		Me.shotYPos = Nothing
		Me.spawnRoot1 = Nothing
		Me.spawnRoot2 = Nothing
		Me.boomerang = Nothing
		Me.boomerangPink = Nothing
		Me.pinkString = Nothing
		Me.damageDealer = Nothing
		Me.main = Nothing
		Me.mainAnimator = Nothing
		Me.collider = Nothing
	End Sub

	' Token: 0x04001CCE RID: 7374
	Private shotYPos As Single() = New Single() { 100F, -50F, -200F }

	' Token: 0x04001CCF RID: 7375
	<SerializeField()>
	Private spawnRoot1 As Transform

	' Token: 0x04001CD0 RID: 7376
	<SerializeField()>
	Private spawnRoot2 As Transform

	' Token: 0x04001CD1 RID: 7377
	<SerializeField()>
	Private boomerang As AirplaneLevelBoomerang

	' Token: 0x04001CD2 RID: 7378
	<SerializeField()>
	Private boomerangPink As AirplaneLevelBoomerang

	' Token: 0x04001CD3 RID: 7379
	Private pinkString As PatternString

	' Token: 0x04001CD4 RID: 7380
	Private damageDealer As DamageDealer

	' Token: 0x04001CD5 RID: 7381
	<SerializeField()>
	Private main As AirplaneLevelBulldogPlane

	' Token: 0x04001CD6 RID: 7382
	<SerializeField()>
	Private mainAnimator As Animator

	' Token: 0x04001CD7 RID: 7383
	Private count As Integer

	' Token: 0x04001CD8 RID: 7384
	<SerializeField()>
	Private collider As GameObject
End Class
