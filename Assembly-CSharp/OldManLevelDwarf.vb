Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000700 RID: 1792
Public Class OldManLevelDwarf
	Inherits AbstractProjectile

	' Token: 0x170003CA RID: 970
	' (get) Token: 0x0600265C RID: 9820 RVA: 0x0016674C File Offset: 0x00164B4C
	' (set) Token: 0x0600265D RID: 9821 RVA: 0x00166754 File Offset: 0x00164B54
	Public Property inPlace As Boolean

	' Token: 0x0600265E RID: 9822 RVA: 0x0016675D File Offset: 0x00164B5D
	Protected Overrides Sub OnDieDistance()
	End Sub

	' Token: 0x0600265F RID: 9823 RVA: 0x0016675F File Offset: 0x00164B5F
	Protected Overrides Sub OnDieLifetime()
	End Sub

	' Token: 0x06002660 RID: 9824 RVA: 0x00166764 File Offset: 0x00164B64
	Protected Overrides Sub Start()
		MyBase.Start()
		Me.startPos = MyBase.transform.position
		Me.damageReceiver = MyBase.GetComponent(Of DamageReceiver)()
		AddHandler Me.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
		Me.damageReceiver.enabled = False
		Me.inPlace = True
	End Sub

	' Token: 0x06002661 RID: 9825 RVA: 0x001667BE File Offset: 0x00164BBE
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x06002662 RID: 9826 RVA: 0x001667DC File Offset: 0x00164BDC
	Public Overrides Sub OnParry(player As AbstractPlayerController)
		Me.Death(True)
	End Sub

	' Token: 0x06002663 RID: 9827 RVA: 0x001667E5 File Offset: 0x00164BE5
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		Me.health -= info.damage
		If Me.health < 0F Then
			Level.Current.RegisterMinionKilled()
			Me.Death(False)
		End If
	End Sub

	' Token: 0x06002664 RID: 9828 RVA: 0x0016681C File Offset: 0x00164C1C
	Private Iterator Function move_up_cr() As IEnumerator
		Dim wait As YieldInstruction = New WaitForFixedUpdate()
		Dim speed As Single = 200F
		Me.rend.sortingLayerID = SortingLayer.NameToID("Default")
		Me.rend.sortingOrder = 2
		While MyBase.transform.position.y < -430F
			MyBase.transform.AddPosition(0F, speed * CupheadTime.FixedDelta, 0F)
			Yield wait
		End While
		Me.beardController.CueRuffle(Me.rufflePos)
		MyBase.animator.SetTrigger("Continue")
		Dim typeString As String = If((Not Me.typeA), "_B", "_A")
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Trans" + typeString + Me.colorString, False, True)
		Yield Nothing
		Return
	End Function

	' Token: 0x06002665 RID: 9829 RVA: 0x00166837 File Offset: 0x00164C37
	Public Overrides Sub SetParryable(parryable As Boolean)
		MyBase.SetParryable(parryable)
	End Sub

	' Token: 0x06002666 RID: 9830 RVA: 0x00166840 File Offset: 0x00164C40
	Public Sub ShootInArc(apexHeight As Single, timeToApex As Single, health As Single, typeA As Boolean, parryable As Boolean, warningTime As Single)
		Me.apexheight = apexHeight
		Me.apexTime = timeToApex
		Me.health = health
		Me.typeA = typeA
		Me.inPlace = False
		Me.damageReceiver.enabled = True
		Me.parryable = parryable
		Me.warningTime = warningTime
		If parryable Then
			Me.colorString = "_Pink"
		Else
			Me.colorString = If((Not Me.isBlue), "_Teal", String.Empty)
			Me.isBlue = Not Me.isBlue
		End If
		Me.SetParryable(False)
		MyBase.StartCoroutine(Me.arc_cr())
	End Sub

	' Token: 0x06002667 RID: 9831 RVA: 0x001668E8 File Offset: 0x00164CE8
	Private Sub CalculateArc()
		Dim num As Single = Me.apexheight
		Dim num2 As Single = Me.apexTime * Me.apexTime
		Dim num3 As Single = -2F * num / num2
		Dim num4 As Single = 2F * num / Me.apexTime
		Dim num5 As Single = num4 * num4
		Dim position As Vector3 = MyBase.transform.position
		Dim position2 As Vector3 = PlayerManager.GetNext().transform.position
		Dim num6 As Single = position2.x - position.x
		Dim num7 As Single = position2.y - position.y
		Dim num8 As Single = num5 + 2F * num3 * num7
		If num8 < 0F Then
			num8 = 0F
		End If
		Dim num9 As Single = (-num4 + Mathf.Sqrt(num8)) / num3
		Dim num10 As Single = (-num4 - Mathf.Sqrt(num8)) / num3
		Dim num11 As Single = Mathf.Max(num9, num10)
		Me.vel.x = num6 / num11
		Me.vel.y = num4
		Me.gravity = num3
	End Sub

	' Token: 0x06002668 RID: 9832 RVA: 0x001669D4 File Offset: 0x00164DD4
	Private Iterator Function arc_cr() As IEnumerator
		Dim finishedArcing As Boolean = False
		Me.inPlace = False
		Dim typeString As String = If((Not Me.typeA), "_B", "_A")
		MyBase.animator.Play("Climb" + typeString + Me.colorString)
		MyBase.GetComponent(Of SpriteRenderer)().sortingOrder = 2
		Yield MyBase.StartCoroutine(Me.move_up_cr())
		Dim beardPopPrefab As Effect = If((Not Me.typeA), Me.beardPopB, Me.beardPopA)
		Me.beardPop = beardPopPrefab.Create(New Vector3(MyBase.transform.position.x, -335F))
		Yield Nothing
		Yield Me.beardPop.animator.WaitForAnimationToStart(Me, "Pimple_Warning", False)
		AudioManager.Play("sfx_dlc_omm_gnome_groundstretch")
		Me.emitAudioFromObject.Add("sfx_dlc_omm_gnome_groundstretch")
		Yield CupheadTime.WaitForSeconds(Me, Me.warningTime)
		Me.CalculateArc()
		Me.SetParryable(Me.parryable)
		Me.coll.enabled = True
		MyBase.transform.position = New Vector3(MyBase.transform.position.x, -325F)
		MyBase.transform.localScale = New Vector3(Mathf.Sign(Me.vel.x), 1F)
		MyBase.animator.Play("Spin" + typeString + Me.colorString)
		Me.rend.sortingLayerID = SortingLayer.NameToID("Player")
		Me.rend.sortingOrder = 50
		AudioManager.Play("sfx_dlc_omm_gnome_groundstretchpop")
		Me.emitAudioFromObject.Add("sfx_dlc_omm_gnome_groundstretchpop")
		AudioManager.Play("sfx_dlc_omm_gnome_somersault_voice")
		Me.emitAudioFromObject.Add("sfx_dlc_omm_gnome_somersault_voice")
		AudioManager.Play("sfx_dlc_omm_gnome_somersault")
		Me.emitAudioFromObject.Add("sfx_dlc_omm_gnome_somersault")
		Me.beardPop.animator.Play("Pimple_End")
		Me.groundShadow = True
		Me.currentArcTime = 0F
		Dim wait As YieldInstruction = New WaitForFixedUpdate()
		While Not finishedArcing
			Me.vel += New Vector3(0F, Me.gravity * CupheadTime.FixedDelta)
			MyBase.transform.Translate(Me.vel * CupheadTime.FixedDelta)
			If Me.rend.sortingOrder = 50 AndAlso Me.vel.y < 0F Then
				Me.rend.sortingLayerID = SortingLayer.NameToID("Enemies")
				Me.rend.sortingOrder = 4
			End If
			If Me.vel.y < 0F AndAlso MyBase.transform.position.y < -289F Then
				finishedArcing = True
				Exit While
			End If
			Me.currentArcTime += CupheadTime.FixedDelta
			Yield wait
		End While
		Me.groundShadow = False
		Dim pos As Vector3 = New Vector3(MyBase.transform.position.x, -289F)
		Dim beardHealPrefab As Effect = If((Not Me.typeA), Me.beardHealB, Me.beardHealA)
		beardHealPrefab.Create(pos + Vector3.down * 25F)
		Me.rend.sortingLayerID = SortingLayer.NameToID("Default")
		Me.rend.sortingOrder = 5
		Me.vel.x = 0F
		Me.vel.y = Me.vel.y * 0.5F
		Dim t As Single = 0F
		While t < 0.041666668F AndAlso MyBase.transform.position.y > -334F
			t += CupheadTime.FixedDelta
			MyBase.transform.Translate(Me.vel * CupheadTime.FixedDelta)
			Yield wait
		End While
		Me.Respawn()
		Yield Nothing
		Return
	End Function

	' Token: 0x06002669 RID: 9833 RVA: 0x001669F0 File Offset: 0x00164DF0
	Public Sub Death(Optional parried As Boolean = False)
		If Me.beardPop Then
			Global.UnityEngine.[Object].Destroy(Me.beardPop.gameObject)
		End If
		If MyBase.transform.position.y > Me.startPos.y Then
			Me.deathPuff.Create(MyBase.transform.position)
		End If
		If Not parried Then
			For i As Integer = 0 To Me.deathParts.Length - 1
				If i <> 0 OrElse Global.UnityEngine.Random.Range(0, 10) = 0 Then
					Dim spriteDeathParts As SpriteDeathParts = Me.deathParts(i).CreatePart(MyBase.transform.position)
					If i <> 0 Then
						spriteDeathParts.animator.Play(Me.colorString)
					End If
				End If
			Next
			AudioManager.Play("sfx_dlc_omm_gnome_popper_death")
			Me.emitAudioFromObject.Add("sfx_dlc_omm_gnome_popper_death")
		End If
		AudioManager.[Stop]("sfx_dlc_omm_gnome_somersault")
		AudioManager.[Stop]("sfx_dlc_omm_gnome_somersault_voice")
		Me.groundShadow = False
		Me.Respawn()
	End Sub

	' Token: 0x0600266A RID: 9834 RVA: 0x00166AF4 File Offset: 0x00164EF4
	Private Sub Respawn()
		Me.StopAllCoroutines()
		Me.damageReceiver.enabled = False
		MyBase.transform.position = Me.startPos
		Me.inPlace = True
		Me.coll.enabled = False
	End Sub

	' Token: 0x0600266B RID: 9835 RVA: 0x00166B2C File Offset: 0x00164F2C
	Private Sub LateUpdate()
		If Me.groundShadow Then
			Me.shadowRend.sortingOrder = 5
			Me.shadowRend.transform.position = New Vector3(MyBase.transform.position.x, -314F + Mathf.Lerp(40F, 60F, Me.currentArcTime / (Me.apexTime * 2F)))
			If MyBase.transform.position.y < -314F + Me.shadowRange Then
				Dim num As Single = Mathf.Lerp(0F, CSng((Me.shadowSprites.Length - 4)), Mathf.InverseLerp(-314F, -314F + Me.shadowRange, MyBase.transform.position.y))
				Me.shadowRend.sprite = Me.shadowSprites(CInt(num))
			Else
				Me.shadowRend.sprite = Me.shadowSprites(Me.shadowSprites.Length - 3 + CInt((Me.currentArcTime * 24F)) Mod 3)
			End If
		Else
			Me.shadowRend.sortingOrder = 1
			Me.shadowRend.transform.localPosition = Vector3.zero
		End If
	End Sub

	' Token: 0x04002EEB RID: 12011
	Private Const START_POS_Y As Single = -430F

	' Token: 0x04002EEC RID: 12012
	Private Const JUMP_POS_Y As Single = -325F

	' Token: 0x04002EED RID: 12013
	Private Const LAND_POS_Y As Single = -314F

	' Token: 0x04002EEE RID: 12014
	Private Const LAND_OFFSET As Single = 25F

	' Token: 0x04002EEF RID: 12015
	Private Const SHADOW_OFFSET_START As Single = 40F

	' Token: 0x04002EF0 RID: 12016
	Private Const SHADOW_OFFSET_END As Single = 60F

	' Token: 0x04002EF1 RID: 12017
	<Header("Death FX")>
	<SerializeField()>
	Private deathPuff As Effect

	' Token: 0x04002EF2 RID: 12018
	<SerializeField()>
	Private deathParts As SpriteDeathParts()

	' Token: 0x04002EF3 RID: 12019
	<Header("Beard FX")>
	<SerializeField()>
	Private beardPopA As Effect

	' Token: 0x04002EF4 RID: 12020
	<SerializeField()>
	Private beardPopB As Effect

	' Token: 0x04002EF5 RID: 12021
	<SerializeField()>
	Private beardHealA As Effect

	' Token: 0x04002EF6 RID: 12022
	<SerializeField()>
	Private beardHealB As Effect

	' Token: 0x04002EF7 RID: 12023
	<SerializeField()>
	Private rend As SpriteRenderer

	' Token: 0x04002EF8 RID: 12024
	<SerializeField()>
	Private coll As Collider2D

	' Token: 0x04002EFA RID: 12026
	Private damageReceiver As DamageReceiver

	' Token: 0x04002EFB RID: 12027
	Private startPos As Vector3

	' Token: 0x04002EFC RID: 12028
	Private vel As Vector3

	' Token: 0x04002EFD RID: 12029
	Private gravity As Single

	' Token: 0x04002EFE RID: 12030
	Private health As Single

	' Token: 0x04002EFF RID: 12031
	Private apexTime As Single

	' Token: 0x04002F00 RID: 12032
	Private bulletSpeed As Single

	' Token: 0x04002F01 RID: 12033
	Private apexheight As Single

	' Token: 0x04002F02 RID: 12034
	Private currentArcTime As Single

	' Token: 0x04002F03 RID: 12035
	Private warningTime As Single

	' Token: 0x04002F04 RID: 12036
	Private typeA As Boolean

	' Token: 0x04002F05 RID: 12037
	Private parryable As Boolean

	' Token: 0x04002F06 RID: 12038
	Private colorString As String

	' Token: 0x04002F07 RID: 12039
	Private isBlue As Boolean = True

	' Token: 0x04002F08 RID: 12040
	Private beardPop As Effect

	' Token: 0x04002F09 RID: 12041
	<SerializeField()>
	Private shadowRange As Single = 100F

	' Token: 0x04002F0A RID: 12042
	<SerializeField()>
	Private shadowRend As SpriteRenderer

	' Token: 0x04002F0B RID: 12043
	<SerializeField()>
	Private shadowSprites As Sprite()

	' Token: 0x04002F0C RID: 12044
	Private groundShadow As Boolean

	' Token: 0x04002F0D RID: 12045
	<SerializeField()>
	Private beardController As OldManLevelBeardController

	' Token: 0x04002F0E RID: 12046
	<SerializeField()>
	Private rufflePos As Integer
End Class
