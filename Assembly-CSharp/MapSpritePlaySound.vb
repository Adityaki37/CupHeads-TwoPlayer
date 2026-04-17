Imports System
Imports UnityEngine

' Token: 0x02000962 RID: 2402
Public Class MapSpritePlaySound
	Inherits AbstractCollidableObject

	' Token: 0x0600380B RID: 14347 RVA: 0x00201491 File Offset: 0x001FF891
	Protected Overrides Sub OnCollision(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollision(hit, phase)
	End Sub

	' Token: 0x0600380C RID: 14348 RVA: 0x0020149C File Offset: 0x001FF89C
	Public Sub PlaySoundRight(isP1 As Boolean)
		Dim soundToPlay As MapSpritePlaySound.SoundToPlay = Me.getSound
		If soundToPlay <> MapSpritePlaySound.SoundToPlay.Wood Then
			If soundToPlay <> MapSpritePlaySound.SoundToPlay.Rainbow Then
			End If
		Else
			AudioManager.Play(If((Not isP1), "player_map_walk_wood_two_p2", "player_map_walk_wood_two_p1"))
		End If
	End Sub

	' Token: 0x0600380D RID: 14349 RVA: 0x002014E8 File Offset: 0x001FF8E8
	Public Sub PlaySoundLeft(isP1 As Boolean)
		Dim soundToPlay As MapSpritePlaySound.SoundToPlay = Me.getSound
		If soundToPlay <> MapSpritePlaySound.SoundToPlay.Wood Then
			If soundToPlay <> MapSpritePlaySound.SoundToPlay.Rainbow Then
			End If
		Else
			AudioManager.Play(If((Not isP1), "player_map_walk_wood_one_p2", "player_map_walk_wood_one_p1"))
		End If
	End Sub

	' Token: 0x04003FED RID: 16365
	Public getSound As MapSpritePlaySound.SoundToPlay

	' Token: 0x02000963 RID: 2403
	Public Enum SoundToPlay
		' Token: 0x04003FEF RID: 16367
		Wood
		' Token: 0x04003FF0 RID: 16368
		Rainbow
	End Enum
End Class
