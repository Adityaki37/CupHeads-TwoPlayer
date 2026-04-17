Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000826 RID: 2086
Public Class TrainLevelPumpkin
	Inherits AbstractCollidableObject

	' Token: 0x0600306C RID: 12396 RVA: 0x001C8870 File Offset: 0x001C6C70
	Public Sub Create(pos As Vector2, direction As Integer, speed As Single, health As Single, fallTime As Single, target As Transform)
		Dim trainLevelPumpkin As TrainLevelPumpkin = Me.InstantiatePrefab(Of TrainLevelPumpkin)()
		trainLevelPumpkin.transform.position = pos
		trainLevelPumpkin.transform.SetScale(New Single?(CSng((-CSng(direction)))), New Single?(1F), New Single?(1F))
		trainLevelPumpkin.direction = direction
		trainLevelPumpkin.health = health
		trainLevelPumpkin.speed = speed
		trainLevelPumpkin.target = target
		trainLevelPumpkin.fallTime = fallTime
	End Sub

	' Token: 0x0600306D RID: 12397 RVA: 0x001C88E4 File Offset: 0x001C6CE4
	Private Sub Start()
		MyBase.StartCoroutine(Me.x_cr())
		MyBase.StartCoroutine(Me.drop_cr())
		Me.damageReceiver = MyBase.GetComponent(Of DamageReceiver)()
		AddHandler Me.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
		Me.brick = TryCast(Me.brickPrefab.Create(), TrainLevelPumpkinProjectile)
		Me.brick.fallTime = Me.fallTime
		Me.brick.transform.SetParent(MyBase.transform)
		Me.brick.transform.ResetLocalTransforms()
	End Sub

	' Token: 0x0600306E RID: 12398 RVA: 0x001C897B File Offset: 0x001C6D7B
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		Me.health -= info.damage
		If Me.health <= 0F Then
			Me.Die()
		End If
	End Sub

	' Token: 0x0600306F RID: 12399 RVA: 0x001C89A6 File Offset: 0x001C6DA6
	Private Sub Drop()
		If Me.brick IsNot Nothing Then
			Me.brick.Drop()
			Me.brick = Nothing
			MyBase.StartCoroutine(Me.y_cr())
		End If
	End Sub

	' Token: 0x06003070 RID: 12400 RVA: 0x001C89D8 File Offset: 0x001C6DD8
	Private Sub Die()
		Me.StopAllCoroutines()
		Me.Drop()
		MyBase.animator.Play("Die")
		AudioManager.Play("train_pumpkin_die")
		Me.emitAudioFromObject.Add("train_pumpkin_die")
	End Sub

	' Token: 0x06003071 RID: 12401 RVA: 0x001C8A10 File Offset: 0x001C6E10
	Private Sub OnDeathAnimComplete()
		Me.[End]()
	End Sub

	' Token: 0x06003072 RID: 12402 RVA: 0x001C8A18 File Offset: 0x001C6E18
	Private Sub [End]()
		Me.StopAllCoroutines()
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
	End Sub

	' Token: 0x06003073 RID: 12403 RVA: 0x001C8A2C File Offset: 0x001C6E2C
	Private Iterator Function x_cr() As IEnumerator
		While True
			MyBase.transform.AddPosition(Me.speed * CupheadTime.Delta * CSng(Me.direction), 0F, 0F)
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06003074 RID: 12404 RVA: 0x001C8A48 File Offset: 0x001C6E48
	Private Iterator Function y_cr() As IEnumerator
		Dim ySpeed As Single = 1F
		Dim mult As Single = 1.1F
		While True
			MyBase.transform.AddPosition(0F, ySpeed * CupheadTime.Delta, 0F)
			If CupheadTime.Delta IsNot 0F Then
				ySpeed *= mult
			End If
			Yield Nothing
			If MyBase.transform.position.y > 720F Then
				Me.[End]()
			End If
		End While
		Return
	End Function

	' Token: 0x06003075 RID: 12405 RVA: 0x001C8A64 File Offset: 0x001C6E64
	Private Iterator Function drop_cr() As IEnumerator
		Dim check As Boolean = True
		While check
			If Me.direction > 0 AndAlso MyBase.transform.position.x > Me.target.position.x Then
				check = False
				Me.Drop()
			End If
			If Me.direction < 0 AndAlso MyBase.transform.position.x < Me.target.position.x Then
				check = False
				Me.Drop()
			End If
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06003076 RID: 12406 RVA: 0x001C8A7F File Offset: 0x001C6E7F
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me.brickPrefab = Nothing
	End Sub

	' Token: 0x0400391D RID: 14621
	<SerializeField()>
	Private brickPrefab As TrainLevelPumpkinProjectile

	' Token: 0x0400391E RID: 14622
	Private brick As TrainLevelPumpkinProjectile

	' Token: 0x0400391F RID: 14623
	Private direction As Integer

	' Token: 0x04003920 RID: 14624
	Private speed As Single

	' Token: 0x04003921 RID: 14625
	Private health As Single

	' Token: 0x04003922 RID: 14626
	Private fallTime As Single

	' Token: 0x04003923 RID: 14627
	Private target As Transform

	' Token: 0x04003924 RID: 14628
	Private damageReceiver As DamageReceiver
End Class
