Imports System
Imports UnityEngine

' Token: 0x020006E1 RID: 1761
Public Class MouseLevelCanCatapultProjectile
	Inherits BasicProjectile

	' Token: 0x0600258A RID: 9610 RVA: 0x0015F604 File Offset: 0x0015DA04
	Public Function CreateFromPrefab(pos As Vector2, rotation As Single, speed As Single, c As Char) As MouseLevelCanCatapultProjectile
		Dim mouseLevelCanCatapultProjectile As MouseLevelCanCatapultProjectile = TryCast(MyBase.Create(pos, rotation, speed), MouseLevelCanCatapultProjectile)
		mouseLevelCanCatapultProjectile.[Set](c)
		Return mouseLevelCanCatapultProjectile
	End Function

	' Token: 0x0600258B RID: 9611 RVA: 0x0015F629 File Offset: 0x0015DA29
	Protected Overrides Sub RandomizeVariant()
	End Sub

	' Token: 0x0600258C RID: 9612 RVA: 0x0015F62C File Offset: 0x0015DA2C
	Private Sub [Set](c As Char)
		Dim num As Integer
		Select Case c
			Case "b"c
			Case "c"c
				num = 4
				GoTo IL_0067
			Case Else
				Select Case c
					Case "n"c
						num = 1
						GoTo IL_0067
					Case "p"c
						num = 3
						GoTo IL_0067
				End Select
			Case "g"c
				num = 2
				Me.SetParryable(True)
				GoTo IL_0067
		End Select
		num = 0
		IL_0067:
		Me.SetInt(AbstractProjectile.[Variant], num)
	End Sub
End Class
