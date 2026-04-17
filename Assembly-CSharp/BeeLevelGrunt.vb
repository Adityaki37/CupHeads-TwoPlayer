Imports System
Imports UnityEngine

' Token: 0x02000512 RID: 1298
Public Class BeeLevelGrunt
	Inherits AbstractCollidableObject

	' Token: 0x0600171C RID: 5916 RVA: 0x000CFA78 File Offset: 0x000CDE78
	Public Function Create(pos As Vector2, xScale As Integer, health As Integer, speed As Single) As BeeLevelGrunt
		Dim beeLevelGrunt As BeeLevelGrunt = Global.UnityEngine.[Object].Instantiate(Of BeeLevelGrunt)(Me)
		beeLevelGrunt.speed = speed
		beeLevelGrunt.health = CSng(health)
		beeLevelGrunt.transform.SetScale(New Single?(CSng(xScale)), New Single?(1F), New Single?(1F))
		beeLevelGrunt.transform.position = pos
		Return beeLevelGrunt
	End Function

	' Token: 0x0600171D RID: 5917 RVA: 0x000CFAD4 File Offset: 0x000CDED4
	Protected Overrides Sub Awake()
		MyBase.Awake()
		AddHandler MyBase.GetComponent(Of DamageReceiver)().OnDamageTaken, AddressOf Me.OnDamageTaken
		Me.damageDealer = New DamageDealer(1F, 1F, True, False, False)
	End Sub

	' Token: 0x0600171E RID: 5918 RVA: 0x000CFB0C File Offset: 0x000CDF0C
	Private Sub Update()
		If Me.dead Then
			Return
		End If
		If MyBase.transform.position.x < -1280F OrElse MyBase.transform.position.x > 1280F Then
			Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
			Return
		End If
		MyBase.transform.AddPosition(Me.speed * CupheadTime.Delta * -MyBase.transform.localScale.x, 0F, 0F)
	End Sub

	' Token: 0x0600171F RID: 5919 RVA: 0x000CFBA6 File Offset: 0x000CDFA6
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x06001720 RID: 5920 RVA: 0x000CFBC4 File Offset: 0x000CDFC4
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		Me.health -= info.damage
		If Me.health <= 0F Then
			Me.Die()
		End If
	End Sub

	' Token: 0x06001721 RID: 5921 RVA: 0x000CFBF0 File Offset: 0x000CDFF0
	Private Sub Die()
		AudioManager.Play("level_bee_grunt_death")
		Me.dead = True
		Me.briefcasePrefab.Create(CInt(MyBase.transform.localScale.x), MyBase.transform.position)
		MyBase.GetComponent(Of Collider2D)().enabled = False
		MyBase.animator.Play("Die")
		MyBase.transform.SetEulerAngles(New Single?(0F), New Single?(0F), New Single?(CSng(Global.UnityEngine.Random.Range(0, 360))))
		MyBase.transform.SetScale(New Single?(CSng(MathUtils.PlusOrMinus())), New Single?(CSng(MathUtils.PlusOrMinus())), New Single?(1F))
	End Sub

	' Token: 0x06001722 RID: 5922 RVA: 0x000CFCB5 File Offset: 0x000CE0B5
	Private Sub OnDeathAnimComplete()
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
	End Sub

	' Token: 0x06001723 RID: 5923 RVA: 0x000CFCC2 File Offset: 0x000CE0C2
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me.briefcasePrefab = Nothing
	End Sub

	' Token: 0x04002062 RID: 8290
	<SerializeField()>
	Private briefcasePrefab As BeeLevelGruntBriefcase

	' Token: 0x04002063 RID: 8291
	Private health As Single

	' Token: 0x04002064 RID: 8292
	Private speed As Single

	' Token: 0x04002065 RID: 8293
	Private damageDealer As DamageDealer

	' Token: 0x04002066 RID: 8294
	Private dead As Boolean
End Class
