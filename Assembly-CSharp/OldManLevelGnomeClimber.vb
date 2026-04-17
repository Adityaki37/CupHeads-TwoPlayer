Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000701 RID: 1793
Public Class OldManLevelGnomeClimber
	Inherits AbstractProjectile

	' Token: 0x0600266D RID: 9837 RVA: 0x001674F8 File Offset: 0x001658F8
	Public Overridable Function Init(startXPosition As Single, facing As Single, smashPos As Transform, properties As LevelProperties.OldMan.ClimberGnomes) As OldManLevelGnomeClimber
		MyBase.ResetLifetime()
		MyBase.ResetDistance()
		MyBase.transform.position = New Vector3(startXPosition, -165F)
		Me.smashPos = smashPos
		Me.properties = properties
		MyBase.transform.SetScale(New Single?(facing), Nothing, Nothing)
		Me.smashFXA = Rand.Bool()
		If Not properties.canDestroy Then
			Me.rigidbody.simulated = False
		End If
		Me.hp = properties.health
		AddHandler Me.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
		Me.StartMoving()
		Return Me
	End Function

	' Token: 0x0600266E RID: 9838 RVA: 0x001675A7 File Offset: 0x001659A7
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me.WORKAROUND_NullifyFields()
	End Sub

	' Token: 0x0600266F RID: 9839 RVA: 0x001675B5 File Offset: 0x001659B5
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		Me.hp -= info.damage
		If Me.hp <= 0F Then
			Level.Current.RegisterMinionKilled()
			Me.Die()
		End If
	End Sub

	' Token: 0x06002670 RID: 9840 RVA: 0x001675EC File Offset: 0x001659EC
	Protected Overrides Sub Die()
		Me.deathPuff.Create(MyBase.transform.position)
		Me.deathParts(0).Create(MyBase.transform.position)
		Me.deathParts(1).Create(MyBase.transform.position)
		Dim spriteDeathParts As SpriteDeathParts = Me.hat.CreatePart(MyBase.transform.position)
		spriteDeathParts.animator.Play("_Teal")
		AudioManager.Play("sfx_dlc_omm_gnome_death")
		Me.emitAudioFromObject.Add("sfx_dlc_omm_gnome_death")
		Me.Recycle()
	End Sub

	' Token: 0x06002671 RID: 9841 RVA: 0x00167689 File Offset: 0x00165A89
	Protected Overrides Sub Update()
		MyBase.Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x06002672 RID: 9842 RVA: 0x001676A7 File Offset: 0x00165AA7
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
		MyBase.OnCollisionPlayer(hit, phase)
	End Sub

	' Token: 0x06002673 RID: 9843 RVA: 0x001676C5 File Offset: 0x00165AC5
	Private Sub StartMoving()
		MyBase.StartCoroutine(Me.move_up_cr())
	End Sub

	' Token: 0x06002674 RID: 9844 RVA: 0x001676D4 File Offset: 0x00165AD4
	Private Iterator Function move_up_cr() As IEnumerator
		MyBase.animator.SetBool("DualSmash", Me.properties.dualSmash)
		Dim wait As YieldInstruction = New WaitForFixedUpdate()
		Dim speed As Single = Me.properties.climbSpeed
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Appear", False, True)
		While Me.smashPos IsNot Nothing AndAlso MyBase.transform.position.y < Me.smashPos.position.y + 60F
			MyBase.transform.AddPosition(0F, speed * CupheadTime.FixedDelta, 0F)
			Yield wait
		End While
		MyBase.transform.parent = Me.smashPos
		MyBase.animator.SetTrigger("ReachedTop")
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "ReachedTop", False, True)
		If Me.smashPos IsNot Nothing Then
			MyBase.transform.SetPosition(New Single?(Me.smashPos.position.x), New Single?(Me.smashPos.position.y + 100F), Nothing)
		End If
		Yield CupheadTime.WaitForSeconds(Me, Me.properties.preAttackDelay)
		MyBase.animator.Play("Anticipation")
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Anticipation", False, True)
		Yield CupheadTime.WaitForSeconds(Me, Me.properties.attackDelay)
		MyBase.animator.SetTrigger("Attack")
		Dim vanishAnimation As String = If((Not Me.properties.dualSmash), "Vanish", "Vanish_Flipped")
		Yield MyBase.animator.WaitForAnimationToEnd(Me, vanishAnimation, False, True)
		Me.Recycle()
		Yield Nothing
		Return
	End Function

	' Token: 0x06002675 RID: 9845 RVA: 0x001676F0 File Offset: 0x00165AF0
	Private Sub AniEvent_SpawnEffect(ev As AnimationEvent)
		Dim effect As Effect = Me.smashEffect.Create(MyBase.transform.position + New Vector3(Me.smashRoot.localPosition.x * MyBase.transform.localScale.x * ev.floatParameter, Me.smashRoot.localPosition.y))
		effect.transform.SetScale(New Single?(MyBase.transform.localScale.x * ev.floatParameter), Nothing, Nothing)
		effect.GetComponent(Of Animator)().Play(If((Not Me.smashFXA), "B", "A"))
		Me.smashFXA = Not Me.smashFXA
	End Sub

	' Token: 0x06002676 RID: 9846 RVA: 0x001677D2 File Offset: 0x00165BD2
	Private Sub AnimationEvent_SFX_OMM_Gnome_ClimberHammer()
		AudioManager.Play("sfx_dlc_omm_gnome_climber_attack")
		Me.emitAudioFromObject.Add("sfx_dlc_omm_gnome_climber_attack")
	End Sub

	' Token: 0x06002677 RID: 9847 RVA: 0x001677EE File Offset: 0x00165BEE
	Private Sub AnimationEvent_SFX_OMM_Gnome_ClimberHammerVocal()
		AudioManager.Play("sfx_dlc_omm_gnome_climber_attackvocal")
		Me.emitAudioFromObject.Add("sfx_dlc_omm_gnome_climber_attackvocal")
	End Sub

	' Token: 0x06002678 RID: 9848 RVA: 0x0016780A File Offset: 0x00165C0A
	Private Sub WORKAROUND_NullifyFields()
		Me.deathPuff = Nothing
		Me.deathParts = Nothing
		Me.hat = Nothing
		Me.smashEffect = Nothing
		Me.smashRoot = Nothing
		Me.smashPos = Nothing
		Me.rigidbody = Nothing
	End Sub

	' Token: 0x04002F0F RID: 12047
	Private Const START_Y As Single = -165F

	' Token: 0x04002F10 RID: 12048
	Private Const CLIMB_X_OFFSET As Single = 120F

	' Token: 0x04002F11 RID: 12049
	Private Const TOP_Y_OFFSET As Single = 60F

	' Token: 0x04002F12 RID: 12050
	Private Const SMASH_Y_OFFSET As Single = 100F

	' Token: 0x04002F13 RID: 12051
	<SerializeField()>
	Private deathPuff As Effect

	' Token: 0x04002F14 RID: 12052
	<SerializeField()>
	Private deathParts As Effect()

	' Token: 0x04002F15 RID: 12053
	<SerializeField()>
	Private hat As SpriteDeathPartsDLC

	' Token: 0x04002F16 RID: 12054
	<SerializeField()>
	Private smashEffect As Effect

	' Token: 0x04002F17 RID: 12055
	<SerializeField()>
	Private smashRoot As Transform

	' Token: 0x04002F18 RID: 12056
	Private properties As LevelProperties.OldMan.ClimberGnomes

	' Token: 0x04002F19 RID: 12057
	Private smashPos As Transform

	' Token: 0x04002F1A RID: 12058
	<SerializeField()>
	Private damageReceiver As DamageReceiver

	' Token: 0x04002F1B RID: 12059
	<SerializeField()>
	Private rigidbody As Rigidbody2D

	' Token: 0x04002F1C RID: 12060
	Private smashFXA As Boolean

	' Token: 0x04002F1D RID: 12061
	Private hp As Single
End Class
