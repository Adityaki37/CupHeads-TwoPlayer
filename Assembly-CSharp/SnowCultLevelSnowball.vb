Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020007F4 RID: 2036
Public Class SnowCultLevelSnowball
	Inherits AbstractProjectile

	' Token: 0x06002EC7 RID: 11975 RVA: 0x001B98FC File Offset: 0x001B7CFC
	Public Overridable Function Init(pos As Vector3, gravity As Single, verticalVelocity As Single, horizontalVelocity As Single, properties As LevelProperties.SnowCult.Snowball, main As SnowCultLevelYeti, makeSound As Boolean) As SnowCultLevelSnowball
		MyBase.ResetLifetime()
		MyBase.ResetDistance()
		MyBase.transform.position = pos
		Me.properties = properties
		Me.gravity = gravity
		Me.velocity.x = -horizontalVelocity
		Me.velocity.y = verticalVelocity
		MyBase.transform.localScale = New Vector3(Mathf.Sign(horizontalVelocity), 1F)
		Me.hitGround = False
		Me.main = main
		Me.makeSound = makeSound
		MyBase.animator.Play("Spin", 0, main.GetIceCubeStartFrame() * 0.0625F)
		MyBase.StartCoroutine(Me.move_cr())
		Return Me
	End Function

	' Token: 0x06002EC8 RID: 11976 RVA: 0x001B99AC File Offset: 0x001B7DAC
	Public Overridable Function InitOriginal(pos As Vector3, gravity As Single, speed As Single, angle As Single, properties As LevelProperties.SnowCult.Snowball, main As SnowCultLevelYeti) As SnowCultLevelSnowball
		MyBase.ResetLifetime()
		MyBase.ResetDistance()
		MyBase.transform.position = pos
		Me.speed = speed
		MyBase.transform.localScale = New Vector3(Mathf.Sign(angle - 90F), 1F)
		Me.gravity = gravity
		Me.angle = MathUtils.AngleToDirection(angle)
		Me.properties = properties
		Me.hitGround = False
		Me.main = main
		Me.makeSound = True
		MyBase.animator.Play("Spin", 0, main.GetIceCubeStartFrame() * 0.0625F)
		MyBase.StartCoroutine(Me.move_from_yeti_cr())
		Return Me
	End Function

	' Token: 0x06002EC9 RID: 11977 RVA: 0x001B9A5D File Offset: 0x001B7E5D
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x06002ECA RID: 11978 RVA: 0x001B9A7B File Offset: 0x001B7E7B
	Protected Overrides Sub OnCollisionGround(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionGround(hit, phase)
		Me.SFX_SNOWCULT_IceCubeImpact()
		Me.hitGround = True
	End Sub

	' Token: 0x06002ECB RID: 11979 RVA: 0x001B9A94 File Offset: 0x001B7E94
	Private Sub TriggerGlare()
		If Me.glareCounter = 0 Then
			Me.glares(0).enabled = False
			Me.glares(1).enabled = False
		End If
		Me.glareCounter += 1
		If Me.glareCounter = 3 Then
			Me.glares(0).enabled = True
			Me.glares(1).enabled = True
			Me.glareCounter = 0
		End If
	End Sub

	' Token: 0x06002ECC RID: 11980 RVA: 0x001B9B08 File Offset: 0x001B7F08
	Private Iterator Function move_from_yeti_cr() As IEnumerator
		Dim accumulativeGravity As Single = 0F
		Dim wait As YieldInstruction = New WaitForFixedUpdate()
		While Not Me.hitGround
			MyBase.transform.position += Me.angle * Me.speed * CupheadTime.FixedDelta - New Vector3(0F, accumulativeGravity * CupheadTime.FixedDelta, 0F)
			accumulativeGravity += Me.gravity * CupheadTime.FixedDelta
			Yield wait
		End While
		Me.newProjectiles()
		Me.Recycle()
		Yield Nothing
		Return
	End Function

	' Token: 0x06002ECD RID: 11981 RVA: 0x001B9B24 File Offset: 0x001B7F24
	Private Iterator Function move_cr() As IEnumerator
		Dim wait As YieldInstruction = New WaitForFixedUpdate()
		While Not Me.hitGround
			MyBase.transform.AddPosition(Me.velocity.x * CupheadTime.FixedDelta, Me.velocity.y * CupheadTime.FixedDelta, 0F)
			Me.velocity.y = Me.velocity.y - Me.gravity * CupheadTime.FixedDelta
			Yield wait
		End While
		Me.newProjectiles()
		Me.Recycle()
		Yield Nothing
		Return
	End Function

	' Token: 0x06002ECE RID: 11982 RVA: 0x001B9B40 File Offset: 0x001B7F40
	Private Sub newProjectiles()
		Dim snowCultLevelSnowballExplosion As SnowCultLevelSnowballExplosion = Me.snowballExplosion.Spawn()
		snowCultLevelSnowballExplosion.Init(MyBase.transform.position, Me.size, Me.main)
		Dim num As Single = Time.realtimeSinceStartup Mod 0.0001F
		If Me.size = SnowCultLevelSnowball.Size.Large Then
			Dim snowCultLevelSnowball As SnowCultLevelSnowball = Me.mediumSnowballPrefab.Spawn()
			snowCultLevelSnowball.Init(MyBase.transform.position + Vector3.back * num, Me.properties.mediumGravity, Me.properties.mediumVelocityY, Me.properties.mediumVelocityX, Me.properties, Me.main, False)
			Dim snowCultLevelSnowball2 As SnowCultLevelSnowball = Me.mediumSnowballPrefab.Spawn()
			snowCultLevelSnowball2.Init(MyBase.transform.position + Vector3.forward * num, Me.properties.mediumGravity, Me.properties.mediumVelocityY, -Me.properties.mediumVelocityX, Me.properties, Me.main, True)
		ElseIf Me.size = SnowCultLevelSnowball.Size.Medium Then
			Dim snowCultLevelSnowball3 As SnowCultLevelSnowball = Me.smallSnowballPrefab.Spawn()
			snowCultLevelSnowball3.Init(MyBase.transform.position + Vector3.back * num, Me.properties.smallGravity, Me.properties.smallVelocityY, Me.properties.smallVelocityX, Me.properties, Me.main, True)
			Dim snowCultLevelSnowball4 As SnowCultLevelSnowball = Me.smallSnowballPrefab.Spawn()
			snowCultLevelSnowball4.Init(MyBase.transform.position + Vector3.forward * num, Me.properties.smallGravity, Me.properties.smallVelocityY, -Me.properties.smallVelocityX, Me.properties, Me.main, False)
		End If
	End Sub

	' Token: 0x06002ECF RID: 11983 RVA: 0x001B9D18 File Offset: 0x001B8118
	Private Sub SFX_SNOWCULT_IceCubeImpact()
		If Not Me.makeSound Then
			Return
		End If
		Dim text As String = "_large"
		If Me.size = SnowCultLevelSnowball.Size.Medium Then
			text = "_medium"
		End If
		If Me.size = SnowCultLevelSnowball.Size.Small Then
			text = "_small"
		End If
		AudioManager.Play("sfx_dlc_snowcult_p2_snowmonster_fridge_icecube_impact" + text)
		Me.emitAudioFromObject.Add("sfx_dlc_snowcult_p2_snowmonster_fridge_icecube_impact" + text)
	End Sub

	' Token: 0x04003771 RID: 14193
	<SerializeField()>
	Private smallSnowballPrefab As SnowCultLevelSnowball

	' Token: 0x04003772 RID: 14194
	<SerializeField()>
	Private mediumSnowballPrefab As SnowCultLevelSnowball

	' Token: 0x04003773 RID: 14195
	<SerializeField()>
	Private snowballExplosion As SnowCultLevelSnowballExplosion

	' Token: 0x04003774 RID: 14196
	Public size As SnowCultLevelSnowball.Size

	' Token: 0x04003775 RID: 14197
	Private properties As LevelProperties.SnowCult.Snowball

	' Token: 0x04003776 RID: 14198
	Private velocity As Vector3

	' Token: 0x04003777 RID: 14199
	Private gravity As Single

	' Token: 0x04003778 RID: 14200
	Private speed As Single

	' Token: 0x04003779 RID: 14201
	Private hitGround As Boolean

	' Token: 0x0400377A RID: 14202
	Private makeSound As Boolean

	' Token: 0x0400377B RID: 14203
	Private angle As Vector3

	' Token: 0x0400377C RID: 14204
	<SerializeField()>
	Private glares As SpriteRenderer()

	' Token: 0x0400377D RID: 14205
	Private glareCounter As Integer = 1

	' Token: 0x0400377E RID: 14206
	Private main As SnowCultLevelYeti

	' Token: 0x020007F5 RID: 2037
	Public Enum Size
		' Token: 0x04003780 RID: 14208
		Small
		' Token: 0x04003781 RID: 14209
		Medium
		' Token: 0x04003782 RID: 14210
		Large
	End Enum
End Class
