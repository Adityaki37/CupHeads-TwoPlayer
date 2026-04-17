Imports System
Imports UnityEngine

' Token: 0x0200038F RID: 911
Public Class EaseUtils
	' Token: 0x06000ADA RID: 2778 RVA: 0x00080D8C File Offset: 0x0007F18C
	Public Shared Function EaseInOut(inEase As EaseUtils.EaseType, outEase As EaseUtils.EaseType, start As Single, [end] As Single, value As Single) As Single
		If value < 0.5F Then
			Dim num As Single = Mathf.Clamp(value * 2F, 0F, 1F)
			Dim num2 As Single = Mathf.Lerp(start, [end], 0.5F)
			Return EaseUtils.Ease(inEase, start, num2, num)
		End If
		If value > 0.5F Then
			Dim num As Single = Mathf.Clamp(value * 2F - 1F, 0F, 1F)
			Dim num3 As Single = Mathf.Lerp(start, [end], 0.5F)
			Return EaseUtils.Ease(outEase, num3, [end], num)
		End If
		Return Mathf.Lerp(start, [end], 0.5F)
	End Function

	' Token: 0x06000ADB RID: 2779 RVA: 0x00080E28 File Offset: 0x0007F228
	Public Shared Function Ease(ease As EaseUtils.EaseType, start As Single, [end] As Single, value As Single) As Single
		Select Case ease
			Case EaseUtils.EaseType.easeInQuad
				Return EaseUtils.EaseInQuad(start, [end], value)
			Case EaseUtils.EaseType.easeOutQuad
				Return EaseUtils.EaseOutQuad(start, [end], value)
			Case EaseUtils.EaseType.easeInOutQuad
				Return EaseUtils.EaseInOutQuad(start, [end], value)
			Case EaseUtils.EaseType.easeInCubic
				Return EaseUtils.EaseInCubic(start, [end], value)
			Case EaseUtils.EaseType.easeOutCubic
				Return EaseUtils.EaseOutCubic(start, [end], value)
			Case EaseUtils.EaseType.easeInOutCubic
				Return EaseUtils.EaseInOutCubic(start, [end], value)
			Case EaseUtils.EaseType.easeInQuart
				Return EaseUtils.EaseInQuart(start, [end], value)
			Case EaseUtils.EaseType.easeOutQuart
				Return EaseUtils.EaseOutQuart(start, [end], value)
			Case EaseUtils.EaseType.easeInOutQuart
				Return EaseUtils.EaseInOutQuart(start, [end], value)
			Case EaseUtils.EaseType.easeInQuint
				Return EaseUtils.EaseInQuint(start, [end], value)
			Case EaseUtils.EaseType.easeOutQuint
				Return EaseUtils.EaseOutQuint(start, [end], value)
			Case EaseUtils.EaseType.easeInOutQuint
				Return EaseUtils.EaseInOutQuint(start, [end], value)
			Case EaseUtils.EaseType.easeInSine
				Return EaseUtils.EaseInSine(start, [end], value)
			Case EaseUtils.EaseType.easeOutSine
				Return EaseUtils.EaseOutSine(start, [end], value)
			Case EaseUtils.EaseType.easeInOutSine
				Return EaseUtils.EaseInOutSine(start, [end], value)
			Case EaseUtils.EaseType.easeInExpo
				Return EaseUtils.EaseInExpo(start, [end], value)
			Case EaseUtils.EaseType.easeOutExpo
				Return EaseUtils.EaseOutExpo(start, [end], value)
			Case EaseUtils.EaseType.easeInOutExpo
				Return EaseUtils.EaseInOutExpo(start, [end], value)
			Case EaseUtils.EaseType.easeInCirc
				Return EaseUtils.EaseInCirc(start, [end], value)
			Case EaseUtils.EaseType.easeOutCirc
				Return EaseUtils.EaseOutCirc(start, [end], value)
			Case EaseUtils.EaseType.easeInOutCirc
				Return EaseUtils.EaseInOutCirc(start, [end], value)
			Case EaseUtils.EaseType.spring
				Return EaseUtils.Spring(start, [end], value)
			Case EaseUtils.EaseType.easeInBounce
				Return EaseUtils.EaseInBounce(start, [end], value)
			Case EaseUtils.EaseType.easeOutBounce
				Return EaseUtils.EaseOutBounce(start, [end], value)
			Case EaseUtils.EaseType.easeInOutBounce
				Return EaseUtils.EaseInOutBounce(start, [end], value)
			Case EaseUtils.EaseType.easeInBack
				Return EaseUtils.EaseInBack(start, [end], value)
			Case EaseUtils.EaseType.easeOutBack
				Return EaseUtils.EaseOutBack(start, [end], value)
			Case EaseUtils.EaseType.easeInOutBack
				Return EaseUtils.EaseInOutBack(start, [end], value)
			Case EaseUtils.EaseType.easeInElastic
				Return EaseUtils.EaseInElastic(start, [end], value)
			Case EaseUtils.EaseType.easeOutElastic
				Return EaseUtils.EaseOutElastic(start, [end], value)
			Case EaseUtils.EaseType.easeInOutElastic
				Return EaseUtils.EaseInOutElastic(start, [end], value)
		End Select
		Return Mathf.Lerp(start, [end], value)
	End Function

	' Token: 0x06000ADC RID: 2780 RVA: 0x00080FE3 File Offset: 0x0007F3E3
	Public Shared Function Linear(start As Single, [end] As Single, value As Single) As Single
		Return Mathf.Lerp(start, [end], value)
	End Function

	' Token: 0x06000ADD RID: 2781 RVA: 0x00080FF0 File Offset: 0x0007F3F0
	Public Shared Function Clerp(start As Single, [end] As Single, value As Single) As Single
		Dim num As Single = 0F
		Dim num2 As Single = 360F
		Dim num3 As Single = Mathf.Abs((num2 - num) / 2F)
		Dim num5 As Single
		If [end] - start < -num3 Then
			Dim num4 As Single = (num2 - start + [end]) * value
			num5 = start + num4
		ElseIf [end] - start > num3 Then
			Dim num4 As Single = -(num2 - [end] + start) * value
			num5 = start + num4
		Else
			num5 = start + ([end] - start) * value
		End If
		Return num5
	End Function

	' Token: 0x06000ADE RID: 2782 RVA: 0x00081068 File Offset: 0x0007F468
	Public Shared Function Spring(start As Single, [end] As Single, value As Single) As Single
		value = Mathf.Clamp01(value)
		value = (Mathf.Sin(value * 3.1415927F * (0.2F + 2.5F * value * value * value)) * Mathf.Pow(1F - value, 2.2F) + value) * (1F + 1.2F * (1F - value))
		Return start + ([end] - start) * value
	End Function

	' Token: 0x06000ADF RID: 2783 RVA: 0x000810CC File Offset: 0x0007F4CC
	Public Shared Function EaseInQuad(start As Single, [end] As Single, value As Single) As Single
		[end] -= start
		Return [end] * value * value + start
	End Function

	' Token: 0x06000AE0 RID: 2784 RVA: 0x000810DA File Offset: 0x0007F4DA
	Public Shared Function EaseOutQuad(start As Single, [end] As Single, value As Single) As Single
		[end] -= start
		Return-[end] * value * (value - 2F) + start
	End Function

	' Token: 0x06000AE1 RID: 2785 RVA: 0x000810F0 File Offset: 0x0007F4F0
	Public Shared Function EaseInOutQuad(start As Single, [end] As Single, value As Single) As Single
		value /= 0.5F
		[end] -= start
		If value < 1F Then
			Return [end] / 2F * value * value + start
		End If
		value -= 1F
		Return-[end] / 2F * (value * (value - 2F) - 1F) + start
	End Function

	' Token: 0x06000AE2 RID: 2786 RVA: 0x00081147 File Offset: 0x0007F547
	Public Shared Function EaseInCubic(start As Single, [end] As Single, value As Single) As Single
		[end] -= start
		Return [end] * value * value * value + start
	End Function

	' Token: 0x06000AE3 RID: 2787 RVA: 0x00081157 File Offset: 0x0007F557
	Public Shared Function EaseOutCubic(start As Single, [end] As Single, value As Single) As Single
		value -= 1F
		[end] -= start
		Return [end] * (value * value * value + 1F) + start
	End Function

	' Token: 0x06000AE4 RID: 2788 RVA: 0x00081178 File Offset: 0x0007F578
	Public Shared Function EaseInOutCubic(start As Single, [end] As Single, value As Single) As Single
		value /= 0.5F
		[end] -= start
		If value < 1F Then
			Return [end] / 2F * value * value * value + start
		End If
		value -= 2F
		Return [end] / 2F * (value * value * value + 2F) + start
	End Function

	' Token: 0x06000AE5 RID: 2789 RVA: 0x000811CC File Offset: 0x0007F5CC
	Public Shared Function EaseInQuart(start As Single, [end] As Single, value As Single) As Single
		[end] -= start
		Return [end] * value * value * value * value + start
	End Function

	' Token: 0x06000AE6 RID: 2790 RVA: 0x000811DE File Offset: 0x0007F5DE
	Public Shared Function EaseOutQuart(start As Single, [end] As Single, value As Single) As Single
		value -= 1F
		[end] -= start
		Return-[end] * (value * value * value * value - 1F) + start
	End Function

	' Token: 0x06000AE7 RID: 2791 RVA: 0x00081200 File Offset: 0x0007F600
	Public Shared Function EaseInOutQuart(start As Single, [end] As Single, value As Single) As Single
		value /= 0.5F
		[end] -= start
		If value < 1F Then
			Return [end] / 2F * value * value * value * value + start
		End If
		value -= 2F
		Return-[end] / 2F * (value * value * value * value - 2F) + start
	End Function

	' Token: 0x06000AE8 RID: 2792 RVA: 0x00081259 File Offset: 0x0007F659
	Public Shared Function EaseInQuint(start As Single, [end] As Single, value As Single) As Single
		[end] -= start
		Return [end] * value * value * value * value * value + start
	End Function

	' Token: 0x06000AE9 RID: 2793 RVA: 0x0008126D File Offset: 0x0007F66D
	Public Shared Function EaseOutQuint(start As Single, [end] As Single, value As Single) As Single
		value -= 1F
		[end] -= start
		Return [end] * (value * value * value * value * value + 1F) + start
	End Function

	' Token: 0x06000AEA RID: 2794 RVA: 0x00081290 File Offset: 0x0007F690
	Public Shared Function EaseInOutQuint(start As Single, [end] As Single, value As Single) As Single
		value /= 0.5F
		[end] -= start
		If value < 1F Then
			Return [end] / 2F * value * value * value * value * value + start
		End If
		value -= 2F
		Return [end] / 2F * (value * value * value * value * value + 2F) + start
	End Function

	' Token: 0x06000AEB RID: 2795 RVA: 0x000812EC File Offset: 0x0007F6EC
	Public Shared Function EaseInOutArbitraryCoefficient(start As Single, [end] As Single, value As Single, c As Single) As Single
		value /= 0.5F
		[end] -= start
		If value < 1F Then
			Return [end] / 2F * Mathf.Pow(value, c) + start
		End If
		value -= 2F
		Return [end] * 2F + start - [end] / 2F * (Mathf.Pow(Mathf.Abs(value), c - 1F) * Mathf.Abs(value) + 2F)
	End Function

	' Token: 0x06000AEC RID: 2796 RVA: 0x0008135E File Offset: 0x0007F75E
	Public Shared Function EaseInSine(start As Single, [end] As Single, value As Single) As Single
		[end] -= start
		Return-[end] * Mathf.Cos(value / 1F * 1.5707964F) + [end] + start
	End Function

	' Token: 0x06000AED RID: 2797 RVA: 0x0008137E File Offset: 0x0007F77E
	Public Shared Function EaseOutSine(start As Single, [end] As Single, value As Single) As Single
		[end] -= start
		Return [end] * Mathf.Sin(value / 1F * 1.5707964F) + start
	End Function

	' Token: 0x06000AEE RID: 2798 RVA: 0x0008139B File Offset: 0x0007F79B
	Public Shared Function EaseInOutSine(start As Single, [end] As Single, value As Single) As Single
		[end] -= start
		Return-[end] / 2F * (Mathf.Cos(3.1415927F * value / 1F) - 1F) + start
	End Function

	' Token: 0x06000AEF RID: 2799 RVA: 0x000813C5 File Offset: 0x0007F7C5
	Public Shared Function EaseInExpo(start As Single, [end] As Single, value As Single) As Single
		[end] -= start
		Return [end] * Mathf.Pow(2F, 10F * (value / 1F - 1F)) + start
	End Function

	' Token: 0x06000AF0 RID: 2800 RVA: 0x000813ED File Offset: 0x0007F7ED
	Public Shared Function EaseOutExpo(start As Single, [end] As Single, value As Single) As Single
		[end] -= start
		Return [end] * (-Mathf.Pow(2F, -10F * value / 1F) + 1F) + start
	End Function

	' Token: 0x06000AF1 RID: 2801 RVA: 0x00081418 File Offset: 0x0007F818
	Public Shared Function EaseInOutExpo(start As Single, [end] As Single, value As Single) As Single
		value /= 0.5F
		[end] -= start
		If value < 1F Then
			Return [end] / 2F * Mathf.Pow(2F, 10F * (value - 1F)) + start
		End If
		value -= 1F
		Return [end] / 2F * (-Mathf.Pow(2F, -10F * value) + 2F) + start
	End Function

	' Token: 0x06000AF2 RID: 2802 RVA: 0x0008148B File Offset: 0x0007F88B
	Public Shared Function EaseInCirc(start As Single, [end] As Single, value As Single) As Single
		[end] -= start
		Return-[end] * (Mathf.Sqrt(1F - value * value) - 1F) + start
	End Function

	' Token: 0x06000AF3 RID: 2803 RVA: 0x000814AB File Offset: 0x0007F8AB
	Public Shared Function EaseOutCirc(start As Single, [end] As Single, value As Single) As Single
		value -= 1F
		[end] -= start
		Return [end] * Mathf.Sqrt(1F - value * value) + start
	End Function

	' Token: 0x06000AF4 RID: 2804 RVA: 0x000814D0 File Offset: 0x0007F8D0
	Public Shared Function EaseInOutCirc(start As Single, [end] As Single, value As Single) As Single
		value /= 0.5F
		[end] -= start
		If value < 1F Then
			Return-[end] / 2F * (Mathf.Sqrt(1F - value * value) - 1F) + start
		End If
		value -= 2F
		Return [end] / 2F * (Mathf.Sqrt(1F - value * value) + 1F) + start
	End Function

	' Token: 0x06000AF5 RID: 2805 RVA: 0x00081540 File Offset: 0x0007F940
	Public Shared Function EaseInBounce(start As Single, [end] As Single, value As Single) As Single
		[end] -= start
		Dim num As Single = 1F
		Return [end] - EaseUtils.EaseOutBounce(0F, [end], num - value) + start
	End Function

	' Token: 0x06000AF6 RID: 2806 RVA: 0x0008156C File Offset: 0x0007F96C
	Public Shared Function EaseOutBounce(start As Single, [end] As Single, value As Single) As Single
		value /= 1F
		[end] -= start
		If value < 0.36363637F Then
			Return [end] * (7.5625F * value * value) + start
		End If
		If value < 0.72727275F Then
			value -= 0.54545456F
			Return [end] * (7.5625F * value * value + 0.75F) + start
		End If
		If CDbl(value) < 0.9090909090909091 Then
			value -= 0.8181818F
			Return [end] * (7.5625F * value * value + 0.9375F) + start
		End If
		value -= 0.95454544F
		Return [end] * (7.5625F * value * value + 0.984375F) + start
	End Function

	' Token: 0x06000AF7 RID: 2807 RVA: 0x00081614 File Offset: 0x0007FA14
	Public Shared Function EaseInOutBounce(start As Single, [end] As Single, value As Single) As Single
		[end] -= start
		Dim num As Single = 1F
		If value < num / 2F Then
			Return EaseUtils.EaseInBounce(0F, [end], value * 2F) * 0.5F + start
		End If
		Return EaseUtils.EaseOutBounce(0F, [end], value * 2F - num) * 0.5F + [end] * 0.5F + start
	End Function

	' Token: 0x06000AF8 RID: 2808 RVA: 0x00081678 File Offset: 0x0007FA78
	Public Shared Function EaseInBack(start As Single, [end] As Single, value As Single) As Single
		[end] -= start
		value /= 1F
		Dim num As Single = 1.70158F
		Return [end] * value * value * ((num + 1F) * value - num) + start
	End Function

	' Token: 0x06000AF9 RID: 2809 RVA: 0x000816AC File Offset: 0x0007FAAC
	Public Shared Function EaseOutBack(start As Single, [end] As Single, value As Single) As Single
		Dim num As Single = 1.70158F
		[end] -= start
		value = value / 1F - 1F
		Return [end] * (value * value * ((num + 1F) * value + num) + 1F) + start
	End Function

	' Token: 0x06000AFA RID: 2810 RVA: 0x000816EC File Offset: 0x0007FAEC
	Public Shared Function EaseInOutBack(start As Single, [end] As Single, value As Single) As Single
		Dim num As Single = 1.70158F
		[end] -= start
		value /= 0.5F
		If value < 1F Then
			num *= 1.525F
			Return [end] / 2F * (value * value * ((num + 1F) * value - num)) + start
		End If
		value -= 2F
		num *= 1.525F
		Return [end] / 2F * (value * value * ((num + 1F) * value + num) + 2F) + start
	End Function

	' Token: 0x06000AFB RID: 2811 RVA: 0x0008176C File Offset: 0x0007FB6C
	Public Shared Function Punch(amplitude As Single, value As Single) As Single
		If value = 0F Then
			Return 0F
		End If
		If value = 1F Then
			Return 0F
		End If
		Dim num As Single = 0.3F
		Dim num2 As Single = num / 6.2831855F * Mathf.Asin(0F)
		Return amplitude * Mathf.Pow(2F, -10F * value) * Mathf.Sin((value * 1F - num2) * 6.2831855F / num)
	End Function

	' Token: 0x06000AFC RID: 2812 RVA: 0x000817E4 File Offset: 0x0007FBE4
	Public Shared Function EaseInElastic(start As Single, [end] As Single, value As Single) As Single
		[end] -= start
		Dim num As Single = 1F
		Dim num2 As Single = num * 0.3F
		Dim num3 As Single = 0F
		If value = 0F Then
			Return start
		End If
		Dim num4 As Single = value / num
		value = num4
		If num4 = 1F Then
			Return start + [end]
		End If
		Dim num5 As Single
		If num3 = 0F OrElse num3 < Mathf.Abs([end]) Then
			num3 = [end]
			num5 = num2 / 4F
		Else
			num5 = num2 / 6.2831855F * Mathf.Asin([end] / num3)
		End If
		Dim num6 As Single = num3
		Dim num7 As Single = 2F
		Dim num8 As Single = 10F
		Dim num9 As Single = value - 1F
		value = num9
		Return-(num6 * Mathf.Pow(num7, num8 * num9) * Mathf.Sin((value * num - num5) * 6.2831855F / num2)) + start
	End Function

	' Token: 0x06000AFD RID: 2813 RVA: 0x0008189C File Offset: 0x0007FC9C
	Public Shared Function EaseOutElastic(start As Single, [end] As Single, value As Single) As Single
		[end] -= start
		Dim num As Single = 1F
		Dim num2 As Single = num * 0.3F
		Dim num3 As Single = 0F
		If value = 0F Then
			Return start
		End If
		Dim num4 As Single = value / num
		value = num4
		If num4 = 1F Then
			Return start + [end]
		End If
		Dim num5 As Single
		If num3 = 0F OrElse num3 < Mathf.Abs([end]) Then
			num3 = [end]
			num5 = num2 / 4F
		Else
			num5 = num2 / 6.2831855F * Mathf.Asin([end] / num3)
		End If
		Return num3 * Mathf.Pow(2F, -10F * value) * Mathf.Sin((value * num - num5) * 6.2831855F / num2) + [end] + start
	End Function

	' Token: 0x06000AFE RID: 2814 RVA: 0x0008194C File Offset: 0x0007FD4C
	Public Shared Function EaseInOutElastic(start As Single, [end] As Single, value As Single) As Single
		[end] -= start
		Dim num As Single = 1F
		Dim num2 As Single = num * 0.3F
		Dim num3 As Single = 0F
		If value = 0F Then
			Return start
		End If
		Dim num4 As Single = value / (num / 2F)
		value = num4
		If num4 = 2F Then
			Return start + [end]
		End If
		Dim num5 As Single
		If num3 = 0F OrElse num3 < Mathf.Abs([end]) Then
			num3 = [end]
			num5 = num2 / 4F
		Else
			num5 = num2 / 6.2831855F * Mathf.Asin([end] / num3)
		End If
		If value < 1F Then
			Dim num6 As Single = -0.5F
			Dim num7 As Single = num3
			Dim num8 As Single = 2F
			Dim num9 As Single = 10F
			Dim num10 As Single = value - 1F
			value = num10
			Return num6 * (num7 * Mathf.Pow(num8, num9 * num10) * Mathf.Sin((value * num - num5) * 6.2831855F / num2)) + start
		End If
		Dim num11 As Single = num3
		Dim num12 As Single = 2F
		Dim num13 As Single = -10F
		Dim num14 As Single = value - 1F
		value = num14
		Return num11 * Mathf.Pow(num12, num13 * num14) * Mathf.Sin((value * num - num5) * 6.2831855F / num2) * 0.5F + [end] + start
	End Function

	' Token: 0x04001494 RID: 5268
	Public Const EaseOutBounceTime As Single = 0.36363637F

	' Token: 0x02000390 RID: 912
	Public Enum EaseType
		' Token: 0x04001496 RID: 5270
		easeInQuad
		' Token: 0x04001497 RID: 5271
		easeOutQuad
		' Token: 0x04001498 RID: 5272
		easeInOutQuad
		' Token: 0x04001499 RID: 5273
		easeInCubic
		' Token: 0x0400149A RID: 5274
		easeOutCubic
		' Token: 0x0400149B RID: 5275
		easeInOutCubic
		' Token: 0x0400149C RID: 5276
		easeInQuart
		' Token: 0x0400149D RID: 5277
		easeOutQuart
		' Token: 0x0400149E RID: 5278
		easeInOutQuart
		' Token: 0x0400149F RID: 5279
		easeInQuint
		' Token: 0x040014A0 RID: 5280
		easeOutQuint
		' Token: 0x040014A1 RID: 5281
		easeInOutQuint
		' Token: 0x040014A2 RID: 5282
		easeInSine
		' Token: 0x040014A3 RID: 5283
		easeOutSine
		' Token: 0x040014A4 RID: 5284
		easeInOutSine
		' Token: 0x040014A5 RID: 5285
		easeInExpo
		' Token: 0x040014A6 RID: 5286
		easeOutExpo
		' Token: 0x040014A7 RID: 5287
		easeInOutExpo
		' Token: 0x040014A8 RID: 5288
		easeInCirc
		' Token: 0x040014A9 RID: 5289
		easeOutCirc
		' Token: 0x040014AA RID: 5290
		easeInOutCirc
		' Token: 0x040014AB RID: 5291
		linear
		' Token: 0x040014AC RID: 5292
		spring
		' Token: 0x040014AD RID: 5293
		easeInBounce
		' Token: 0x040014AE RID: 5294
		easeOutBounce
		' Token: 0x040014AF RID: 5295
		easeInOutBounce
		' Token: 0x040014B0 RID: 5296
		easeInBack
		' Token: 0x040014B1 RID: 5297
		easeOutBack
		' Token: 0x040014B2 RID: 5298
		easeInOutBack
		' Token: 0x040014B3 RID: 5299
		easeInElastic
		' Token: 0x040014B4 RID: 5300
		easeOutElastic
		' Token: 0x040014B5 RID: 5301
		easeInOutElastic
		' Token: 0x040014B6 RID: 5302
		punch
	End Enum
End Class
