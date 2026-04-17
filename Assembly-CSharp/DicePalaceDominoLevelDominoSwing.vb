Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020005B7 RID: 1463
Public Class DicePalaceDominoLevelDominoSwing
	Inherits AbstractCollidableObject

	' Token: 0x06001C6D RID: 7277 RVA: 0x001044D7 File Offset: 0x001028D7
	Protected Overrides Sub Awake()
		MyBase.Awake()
	End Sub

	' Token: 0x06001C6E RID: 7278 RVA: 0x001044E0 File Offset: 0x001028E0
	Public Sub InitSwing(properties As LevelProperties.DicePalaceDomino)
		Me.speed = properties.CurrentState.domino.swingSpeed
		Me.strength = properties.CurrentState.domino.swingDistance
		Me.origin = New Vector3(MyBase.transform.position.x, properties.CurrentState.domino.swingPosY)
		MyBase.transform.position = Me.origin
		MyBase.StartCoroutine(Me.move_cr())
	End Sub

	' Token: 0x06001C6F RID: 7279 RVA: 0x00104565 File Offset: 0x00102965
	Protected Overridable Function hitPauseCoefficient() As Single
		Return If((Not MyBase.GetComponentInChildren(Of DamageReceiver)().IsHitPaused), 1F, 0F)
	End Function

	' Token: 0x06001C70 RID: 7280 RVA: 0x00104588 File Offset: 0x00102988
	Private Iterator Function move_cr() As IEnumerator
		Yield Me.domino.GetComponent(Of Animator)().WaitForAnimationToEnd(Me, "Intro", False, True)
		Dim angle As Single = 0F
		While True
			angle += Me.speed * CupheadTime.Delta * Me.hitPauseCoefficient()
			MyBase.transform.position = Me.origin + Vector3.up * (Mathf.Sin(angle) * Me.strength)
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x04002568 RID: 9576
	<SerializeField()>
	Private domino As DicePalaceDominoLevelDomino

	' Token: 0x04002569 RID: 9577
	Private speed As Single

	' Token: 0x0400256A RID: 9578
	Private strength As Single

	' Token: 0x0400256B RID: 9579
	Private origin As Vector3
End Class
