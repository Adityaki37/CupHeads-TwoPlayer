Imports System

' Token: 0x02000A55 RID: 2645
Public Class PlayerSuperChaliceShmupBullet
	Inherits BasicProjectile

	' Token: 0x1700056E RID: 1390
	' (get) Token: 0x06003F0E RID: 16142 RVA: 0x002289C9 File Offset: 0x00226DC9
	Protected Overrides ReadOnly Property DestroyLifetime As Single
		Get
			Return Me.lifetimeMax
		End Get
	End Property

	' Token: 0x04004625 RID: 17957
	Public lifetimeMax As Single = 20F
End Class
