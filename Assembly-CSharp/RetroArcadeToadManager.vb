Imports System
Imports UnityEngine

' Token: 0x0200075F RID: 1887
Public Class RetroArcadeToadManager
	Inherits LevelProperties.RetroArcade.Entity

	' Token: 0x0600291E RID: 10526 RVA: 0x0017FA34 File Offset: 0x0017DE34
	Public Sub StartToad()
		Me.p = MyBase.properties.CurrentState.toad
		Me.numDied = 0
		Me.toad1 = Me.toadPrefab.Create(Me, Me.p, True)
		Me.toad2 = Me.toadPrefab.Create(Me, Me.p, False)
	End Sub

	' Token: 0x0600291F RID: 10527 RVA: 0x0017FA90 File Offset: 0x0017DE90
	Public Sub OnToadDie()
		Me.numDied += 1
		If Me.numDied >= 2 Then
			Me.StopAllCoroutines()
			Global.UnityEngine.[Object].Destroy(Me.toad1.gameObject)
			Global.UnityEngine.[Object].Destroy(Me.toad2.gameObject)
			MyBase.properties.DealDamageToNextNamedState()
		End If
	End Sub

	' Token: 0x04003212 RID: 12818
	<SerializeField()>
	Private toadPrefab As RetroArcadeToad

	' Token: 0x04003213 RID: 12819
	Private p As LevelProperties.RetroArcade.Toad

	' Token: 0x04003214 RID: 12820
	Private toad1 As RetroArcadeToad

	' Token: 0x04003215 RID: 12821
	Private toad2 As RetroArcadeToad

	' Token: 0x04003216 RID: 12822
	Private numDied As Integer
End Class
