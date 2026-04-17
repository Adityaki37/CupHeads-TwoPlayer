Imports System

' Token: 0x02000859 RID: 2137
Public Class VeggiesLevelSpit
	Inherits BasicProjectile

	' Token: 0x06003191 RID: 12689 RVA: 0x001CED44 File Offset: 0x001CD144
	Protected Overrides Sub Die()
		If MyBase.CanParry Then
			AudioManager.Play("level_veggies_potato_worm_explode")
		End If
		MyBase.Die()
	End Sub
End Class
