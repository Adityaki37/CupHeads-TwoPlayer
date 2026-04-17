Imports System
Imports UnityEngine

' Token: 0x0200070A RID: 1802
<Serializable()>
Public Class OldManLevelPlatform
	' Token: 0x04002F8A RID: 12170
	Public platform As Transform

	' Token: 0x04002F8B RID: 12171
	Public sockBulletPos As Transform

	' Token: 0x04002F8C RID: 12172
	Public isMoving As Boolean

	' Token: 0x04002F8D RID: 12173
	Public removed As Boolean

	' Token: 0x04002F8E RID: 12174
	Public effectiveVel As Single

	' Token: 0x04002F8F RID: 12175
	Public activeClimber As OldManLevelGnomeClimber
End Class
