Imports System
Imports UnityEngine

' Token: 0x020008D7 RID: 2263
Public Class MountainLevelGiantPlatform
	Inherits LevelPlatform

	' Token: 0x060034EC RID: 13548 RVA: 0x001EC780 File Offset: 0x001EAB80
	Protected Overrides Sub OnCollisionEnemy(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionEnemy(hit, phase)
		If phase = CollisionPhase.Enter AndAlso hit.GetComponent(Of MountainPlatformingLevelCyclops)() Then
			Me.explosion.Create(MyBase.transform.position)
			Me.SpawnParts()
			If MyBase.transform.childCount > 0 AndAlso MyBase.GetComponentInChildren(Of LevelPlayerMotor)() Then
				MyBase.GetComponentInChildren(Of LevelPlayerMotor)().OnPitKnockUp(10F, 1F)
			End If
			Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
		End If
	End Sub

	' Token: 0x060034ED RID: 13549 RVA: 0x001EC80C File Offset: 0x001EAC0C
	Private Sub SpawnParts()
		For Each spriteDeathParts As SpriteDeathParts In Me.sprites
			spriteDeathParts.CreatePart(MyBase.transform.position)
		Next
	End Sub

	' Token: 0x04003D1E RID: 15646
	<SerializeField()>
	Private explosion As Effect

	' Token: 0x04003D1F RID: 15647
	<SerializeField()>
	Private sprites As SpriteDeathParts()
End Class
