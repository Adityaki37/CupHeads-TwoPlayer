Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020007E5 RID: 2021
Public Class SnowCultLevelBurstEffect
	Inherits Effect

	' Token: 0x06002E42 RID: 11842 RVA: 0x001B46B4 File Offset: 0x001B2AB4
	Public Function Create(pos As Vector3, direction As Single) As SnowCultLevelBurstEffect
		Dim snowCultLevelBurstEffect As SnowCultLevelBurstEffect = TryCast(MyBase.Create(pos), SnowCultLevelBurstEffect)
		snowCultLevelBurstEffect.direction = direction
		Return snowCultLevelBurstEffect
	End Function

	' Token: 0x06002E43 RID: 11843 RVA: 0x001B46D8 File Offset: 0x001B2AD8
	Private Sub Start()
		Me.startPosY = MyBase.transform.position.y
		If Me.isSnowFall Then
			MyBase.StartCoroutine(Me.move_cr())
		End If
	End Sub

	' Token: 0x06002E44 RID: 11844 RVA: 0x001B4718 File Offset: 0x001B2B18
	Private Sub SpawnEffect()
		Dim vector As Vector3 = New Vector3(MyBase.transform.position.x + 127F * Me.direction, If((Not Me.isSnowFall), 95F, Me.startPosY))
		If vector.x > -740F AndAlso vector.x < 740F Then
			If Me.isTypeA Then
				Me.typeA.Create(vector, Me.direction)
			Else
				Me.typeB.Create(vector, Me.direction)
			End If
		End If
	End Sub

	' Token: 0x06002E45 RID: 11845 RVA: 0x001B47C0 File Offset: 0x001B2BC0
	Private Iterator Function move_cr() As IEnumerator
		While MyBase.transform.position.y > -360F
			MyBase.transform.position += Vector3.down * 150F * CupheadTime.Delta
			Yield Nothing
		End While
		Me.OnEffectComplete()
		Yield Nothing
		Return
	End Function

	' Token: 0x040036C5 RID: 14021
	Private Const DIST_X_TO_MOVE As Single = 127F

	' Token: 0x040036C6 RID: 14022
	Private Const Y_TO_SPAWN As Single = 95F

	' Token: 0x040036C7 RID: 14023
	Private Const MOVE_SPEED As Single = 150F

	' Token: 0x040036C8 RID: 14024
	<SerializeField()>
	Private isSnowFall As Boolean

	' Token: 0x040036C9 RID: 14025
	<SerializeField()>
	Private isTypeA As Boolean

	' Token: 0x040036CA RID: 14026
	<SerializeField()>
	Private typeA As SnowCultLevelBurstEffect

	' Token: 0x040036CB RID: 14027
	<SerializeField()>
	Private typeB As SnowCultLevelBurstEffect

	' Token: 0x040036CC RID: 14028
	Private startPosY As Single

	' Token: 0x040036CD RID: 14029
	Private direction As Single
End Class
