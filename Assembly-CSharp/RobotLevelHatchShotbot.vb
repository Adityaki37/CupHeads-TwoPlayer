Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x0200077E RID: 1918
Public Class RobotLevelHatchShotbot
	Inherits AbstractCollidableObject

	' Token: 0x06002A16 RID: 10774 RVA: 0x00189680 File Offset: 0x00187A80
	Public Sub InitShotbot(hp As Integer, bulletSpeed As Integer, pinkBulletCount As Integer, shootDelay As Single, flightSpeed As Integer)
		Me.speedPCT = 200F / CSng(flightSpeed)
		Me.health = CSng(hp)
		Me.flightSpeed = flightSpeed
		Me.bulletSpeed = bulletSpeed
		Me.pinkBulletCount = pinkBulletCount
		Me.shotsFired = 0
		Me.shootDelay = shootDelay
		Me.damageDealer = DamageDealer.NewEnemy()
		AddHandler MyBase.GetComponent(Of DamageReceiver)().OnDamageTaken, AddressOf Me.OnDamageTaken
		MyBase.StartCoroutine(Me.rotate_cr())
		MyBase.StartCoroutine(Me.move_cr())
		MyBase.StartCoroutine(Me.intro_cr())
	End Sub

	' Token: 0x06002A17 RID: 10775 RVA: 0x00189714 File Offset: 0x00187B14
	Public Function Create() As RobotLevelHatchShotbot
		Dim gameObject As GameObject = Global.UnityEngine.[Object].Instantiate(Of GameObject)(MyBase.gameObject)
		gameObject.transform.SetEulerAngles(New Single?(0F), New Single?(0F), New Single?(180F))
		Return gameObject.GetComponent(Of RobotLevelHatchShotbot)()
	End Function

	' Token: 0x06002A18 RID: 10776 RVA: 0x0018975C File Offset: 0x00187B5C
	Private Iterator Function intro_cr() As IEnumerator
		Dim rotTime As Single = 0.15F
		Dim scale As Single = MyBase.transform.localScale.x
		MyBase.transform.SetEulerAngles(Nothing, Nothing, New Single?(180F))
		Yield CupheadTime.WaitForSeconds(Me, 0.5F * Me.speedPCT)
		Yield MyBase.StartCoroutine(Me.tweenRotation_cr(180F * scale, 270F * scale, rotTime / 3F * Me.speedPCT))
		Yield MyBase.StartCoroutine(Me.tweenRotation_cr(270F * scale, 180F * scale, rotTime / 3F * Me.speedPCT))
		Yield Nothing
		Return
	End Function

	' Token: 0x06002A19 RID: 10777 RVA: 0x00189778 File Offset: 0x00187B78
	Private Iterator Function move_cr() As IEnumerator
		Dim scale As Single = MyBase.transform.localScale.x
		While True
			Dim move As Vector2 = MyBase.transform.right * CSng(Me.flightSpeed) * CupheadTime.Delta * scale
			MyBase.transform.AddPosition(move.x, move.y, 0F)
			Yield Nothing
			If MyBase.transform.position.y > 460F Then
				Me.[End]()
			End If
		End While
		Return
	End Function

	' Token: 0x06002A1A RID: 10778 RVA: 0x00189794 File Offset: 0x00187B94
	Private Iterator Function rotate_cr() As IEnumerator
		Dim rotTime As Single = 0.15F * Me.speedPCT
		Dim scale As Single = MyBase.transform.localScale.x
		Yield CupheadTime.WaitForSeconds(Me, 1.8F * Me.speedPCT)
		While True
			Yield CupheadTime.WaitForSeconds(Me, Me.time.x * Me.speedPCT)
			Yield MyBase.StartCoroutine(Me.tweenRotation_cr(180F * scale, 90F * scale, rotTime))
			Yield CupheadTime.WaitForSeconds(Me, Me.time.y * Me.speedPCT)
			Yield MyBase.StartCoroutine(Me.tweenRotation_cr(90F * scale, 0F, rotTime))
			Yield CupheadTime.WaitForSeconds(Me, Me.time.x * Me.speedPCT)
			Yield MyBase.StartCoroutine(Me.tweenRotation_cr(0F, 90F * scale, rotTime))
			Yield CupheadTime.WaitForSeconds(Me, Me.time.y * Me.speedPCT)
			Yield MyBase.StartCoroutine(Me.tweenRotation_cr(90F * scale, 180F * scale, rotTime))
		End While
		Return
	End Function

	' Token: 0x06002A1B RID: 10779 RVA: 0x001897B0 File Offset: 0x00187BB0
	Private Iterator Function tweenRotation_cr(start As Single, [end] As Single, time As Single) As IEnumerator
		MyBase.transform.SetEulerAngles(Nothing, Nothing, New Single?(start))
		Dim t As Single = 0F
		While t < time
			Dim val As Single = t / time
			MyBase.transform.SetEulerAngles(Nothing, Nothing, New Single?(EaseUtils.Ease(EaseUtils.EaseType.linear, start, [end], val)))
			t += CupheadTime.Delta / 3F
			Yield Nothing
		End While
		MyBase.transform.SetEulerAngles(Nothing, Nothing, New Single?([end]))
		Return
	End Function

	' Token: 0x06002A1C RID: 10780 RVA: 0x001897E0 File Offset: 0x00187BE0
	Private Iterator Function fire_cr() As IEnumerator
		While True
			Yield CupheadTime.WaitForSeconds(Me, Me.shootDelay)
			AudioManager.Play("robot_shotbot_shoot")
			Me.emitAudioFromObject.Add("robot_shotbot_shoot")
			Dim proj As GameObject = Global.UnityEngine.[Object].Instantiate(Of GameObject)(Me.projectile)
			proj.transform.position = MyBase.transform.position
			proj.transform.right = (PlayerManager.GetNext().center - MyBase.transform.position).normalized
			proj.GetComponent(Of BasicProjectile)().Speed = CSng(Me.bulletSpeed)
			If Me.shotsFired >= Me.pinkBulletCount Then
				Me.shotsFired = 0
				proj.GetComponent(Of SpriteRenderer)().sprite = Me.spriteSpecial
				proj.GetComponent(Of BasicProjectile)().SetParryable(True)
			Else
				Me.shotsFired += 1
			End If
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06002A1D RID: 10781 RVA: 0x001897FB File Offset: 0x00187BFB
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		Me.health -= info.damage
		If Me.health <= 0F Then
			Me.Dead()
		End If
	End Sub

	' Token: 0x06002A1E RID: 10782 RVA: 0x00189826 File Offset: 0x00187C26
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x06002A1F RID: 10783 RVA: 0x00189844 File Offset: 0x00187C44
	Private Sub Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x06002A20 RID: 10784 RVA: 0x0018985C File Offset: 0x00187C5C
	Private Sub Dead()
		AudioManager.Play("robot_shotbot_death")
		Me.emitAudioFromObject.Add("robot_shotbot_death")
		Me.StopAllCoroutines()
		Me.CreateSmoke()
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
	End Sub

	' Token: 0x06002A21 RID: 10785 RVA: 0x0018988F File Offset: 0x00187C8F
	Private Sub [End]()
		Me.StopAllCoroutines()
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
	End Sub

	' Token: 0x06002A22 RID: 10786 RVA: 0x001898A4 File Offset: 0x00187CA4
	Private Sub CreateSmoke()
		Me.smokeEffect.Create(MyBase.transform.position)
		For Each spriteDeathParts As SpriteDeathParts In Me.deathParts
			spriteDeathParts.CreatePart(MyBase.transform.position)
		Next
	End Sub

	' Token: 0x040032F4 RID: 13044
	<SerializeField()>
	Private smokeEffect As Effect

	' Token: 0x040032F5 RID: 13045
	<SerializeField()>
	Private deathParts As SpriteDeathParts()

	' Token: 0x040032F6 RID: 13046
	<SerializeField()>
	Private projectile As GameObject

	' Token: 0x040032F7 RID: 13047
	<SerializeField()>
	Private spriteSpecial As Sprite

	' Token: 0x040032F8 RID: 13048
	<SerializeField()>
	Private time As Vector2

	' Token: 0x040032F9 RID: 13049
	Private speedPCT As Single

	' Token: 0x040032FA RID: 13050
	Private health As Single

	' Token: 0x040032FB RID: 13051
	Private flightSpeed As Integer

	' Token: 0x040032FC RID: 13052
	Private bulletSpeed As Integer

	' Token: 0x040032FD RID: 13053
	Private pinkBulletCount As Integer

	' Token: 0x040032FE RID: 13054
	Private shotsFired As Integer

	' Token: 0x040032FF RID: 13055
	Private shootDelay As Single

	' Token: 0x04003300 RID: 13056
	Private damageDealer As DamageDealer

	' Token: 0x04003301 RID: 13057
	Private Const MAX_HEIGHT As Integer = 460
End Class
