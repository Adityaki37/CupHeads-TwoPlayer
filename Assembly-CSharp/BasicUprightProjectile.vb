Imports System
Imports UnityEngine

' Token: 0x02000AEA RID: 2794
Public Class BasicUprightProjectile
	Inherits BasicProjectile

	' Token: 0x17000608 RID: 1544
	' (get) Token: 0x060043B1 RID: 17329 RVA: 0x001314D3 File Offset: 0x0012F8D3
	Protected Overrides ReadOnly Property Direction As Vector3
		Get
			Return Me._direction
		End Get
	End Property

	' Token: 0x060043B2 RID: 17330 RVA: 0x001314DC File Offset: 0x0012F8DC
	Public Overrides Function Create(position As Vector2, rotation As Single) As AbstractProjectile
		Dim basicUprightProjectile As BasicUprightProjectile = TryCast(MyBase.Create(position, 0F), BasicUprightProjectile)
		basicUprightProjectile._direction = Quaternion.Euler(0F, 0F, rotation) * Vector3.right
		Return basicUprightProjectile
	End Function

	' Token: 0x04004967 RID: 18791
	Private _direction As Vector3
End Class
