Imports System
Imports UnityEngine

' Token: 0x020007F6 RID: 2038
Public Class SnowCultLevelSnowballExplosion
	Inherits MonoBehaviour

	' Token: 0x06002ED1 RID: 11985 RVA: 0x001BA038 File Offset: 0x001B8438
	Public Sub Init(pos As Vector3, size As SnowCultLevelSnowball.Size, main As SnowCultLevelYeti)
		MyBase.transform.position = pos
		If size <> SnowCultLevelSnowball.Size.Large Then
			If size <> SnowCultLevelSnowball.Size.Medium Then
				If size = SnowCultLevelSnowball.Size.Small Then
					Dim smallExplosion As Integer = main.GetSmallExplosion()
					If smallExplosion <> 0 Then
						If smallExplosion <> 1 Then
							If smallExplosion = 2 Then
								Me.animator.Play("SmallC")
							End If
						Else
							Me.animator.Play("SmallB")
						End If
					Else
						Me.animator.Play("SmallA")
					End If
				End If
			Else
				Me.animator.Play(If((main.GetMediumExplosion() <> 0), "MediumB", "MediumA"))
			End If
		Else
			Me.animator.Play("Large")
		End If
	End Sub

	' Token: 0x06002ED2 RID: 11986 RVA: 0x001BA10D File Offset: 0x001B850D
	Private Sub Update()
		If Not Me.rend.enabled Then
			Me.Recycle()
		End If
	End Sub

	' Token: 0x04003783 RID: 14211
	<SerializeField()>
	Private rend As SpriteRenderer

	' Token: 0x04003784 RID: 14212
	<SerializeField()>
	Private animator As Animator
End Class
