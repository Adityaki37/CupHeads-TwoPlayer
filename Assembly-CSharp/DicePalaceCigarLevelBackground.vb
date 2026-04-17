Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020005AD RID: 1453
Public Class DicePalaceCigarLevelBackground
	Inherits AbstractPausableComponent

	' Token: 0x06001C09 RID: 7177 RVA: 0x00101548 File Offset: 0x000FF948
	Private Iterator Function circulate_fire_cr() As IEnumerator
		Dim loopSize As Single = 6F
		Dim angle As Single = 0F
		While True
			angle += 0.5F * CupheadTime.Delta
			Dim handleRotationX As Vector3 = New Vector3(-Mathf.Sin(angle) * loopSize, 0F, 0F)
			Dim handleRotationY As Vector3 = New Vector3(0F, Mathf.Cos(angle) * loopSize, 0F)
			Me.foregroundFireSprite.position = Me.firePivot.position
			Me.foregroundFireSprite.position += handleRotationX + handleRotationY
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x04002518 RID: 9496
	<SerializeField()>
	Private foregroundFireSprite As Transform

	' Token: 0x04002519 RID: 9497
	<SerializeField()>
	Private firePivot As Transform
End Class
