Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x0200068B RID: 1675
Public Class FlyingMermaidLevelHomingProjectile
	Inherits HomingProjectile

	' Token: 0x06002350 RID: 9040 RVA: 0x0014BB64 File Offset: 0x00149F64
	Public Function Create(pos As Vector3, rotation As Single, player As AbstractPlayerController, properties As LevelProperties.FlyingMermaid.HomerFish) As FlyingMermaidLevelHomingProjectile
		Dim flyingMermaidLevelHomingProjectile As FlyingMermaidLevelHomingProjectile = TryCast(MyBase.Create(pos, rotation, properties.initSpeed, properties.bulletSpeed, properties.rotationSpeed, properties.timeBeforeDeath, properties.timeBeforeHoming, player), FlyingMermaidLevelHomingProjectile)
		flyingMermaidLevelHomingProjectile.properties = properties
		flyingMermaidLevelHomingProjectile.transform.position = pos
		Return flyingMermaidLevelHomingProjectile
	End Function

	' Token: 0x06002351 RID: 9041 RVA: 0x0014BBBD File Offset: 0x00149FBD
	Protected Overrides Sub Start()
		MyBase.Start()
		MyBase.StartCoroutine(Me.timer_cr())
	End Sub

	' Token: 0x06002352 RID: 9042 RVA: 0x0014BBD4 File Offset: 0x00149FD4
	Private Iterator Function timer_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 0.2F)
		Me.mainSprite.sortingLayerName = "Foreground"
		Me.mainSprite.sortingOrder = 30
		Yield CupheadTime.WaitForSeconds(Me, Me.properties.timeBeforeDeath - 0.2F)
		MyBase.HomingEnabled = False
		MyBase.animator.SetTrigger("StopTracking")
		While True
			MyBase.transform.position += Me.velocity.normalized * Me.properties.bulletSpeed * 1.4F * CupheadTime.Delta
			MyBase.transform.SetEulerAngles(New Single?(0F), New Single?(0F), New Single?(MathUtils.DirectionToAngle(Me.velocity) + 180F))
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x04002BF2 RID: 11250
	<SerializeField()>
	Private mainSprite As SpriteRenderer

	' Token: 0x04002BF3 RID: 11251
	Private properties As LevelProperties.FlyingMermaid.HomerFish
End Class
