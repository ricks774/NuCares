# NuCares ｜ Who cares? NuCares!!
[專案網址](https://nu-cares.vercel.app) ｜ [前端 Github Repo](https://github.com/Annie-Peng/NuCares) ｜ [Swagger](https://nucares.top/swagger/index.html?url=/swagger/v1/swagger.json) ｜

<hr/>

## 功能介紹
成為會員之後能夠進一步根據使用的服務分為「學員」及「營養師」角色

### 學員
- 註冊/登入
- 搜尋營養師
- 查看營養師專頁、課程方案
- 購買課程方案
- 查看課程列表
- 查看營養師每日菜單
- 編輯每日飲食
- 新增每日身體紀錄
- 查看營養師設定目標
- 查看訂單紀錄
- 收藏營養師
- 修改會員資料
- 修改密碼
- 申請成為營養師

### 營養師
- 登入
- 編輯營養師專頁-關於我
- 編輯課程方案
- 查看學員列表
- 查看學員每日飲食
- 查看學員每日身體紀錄
- 編輯學員目標
- 編輯每日菜單

<hr/>

## 網站體驗流程

### 學員
1. 搜尋營養師 查看營養師專頁與課程方案
2. 購買課程
   選信用卡 -> 填寫卡號 ```4000-2211-1111-1111``` -> 有效年月填大於今天 -> 驗證碼可隨意填寫
3. 查看課程列表
4. 查看營養師提供之菜單份量
5. 編輯每日三餐份量、身體指數


### 營養師
1. 填寫營養師專頁-關於我 （選公開會在 搜尋營養師 介面出現)
2. 課程方案設定課程，讓學員可以依照需求購買需要的課程
3. 查看學員列表，並開始課程
4. 依學員需求設定目標、學員每日三餐份量
5. 查看學員每日飲食狀況、身體紀錄

<hr/>

## 資料夾結構
```jsx
NuCares/
├── public/
│   └── images/
│       ├── footer
│       │   ├── facebook.svg
│       │   ├── instagram.svg
│       │   └── youtube.svg
│       ├── header
│       │   └── menu.svg
│       ├── loading.svg
│       ├── logo.svg
│       ├── emptyPhoto.svg
│       └── ...
├── src/
│   ├── common/
│   │   ├── components/
│   │   │   ├── course/
│   │   │   │   ├── CourseForm.tsx
│   │   │   │   └── CourseFormTr.tsx
│   │   │   ├── modals/
│   │   │   │   ├── MiniModal.tsx
│   │   │   │   ├── TimerModal.tsx
│   │   │   │   └── TitleModal.tsx
│   │   │   ├── dietary-record/
│   │   │   │   ├── bodyRateChart/
│   │   │   │   │   ├── BodyRate.tsx
│   │   │   │   │   └── BodyRateChart.tsx
│   │   │   │   ├── courseInfo/
│   │   │   │   │   ├── CourseInfo.tsx
│   │   │   │   │   └── InfoBtn.tsx
│   │   │   │   ├── dailyRecord/
│   │   │   │   │   ├── DailyDietary.tsx
│   │   │   │   │   ├── FoodDetailForm.tsx
│   │   │   │   │   └── FoodDetailModal.tsx
│   │   │   │   ├── goalChart/
│   │   │   │   │   ├── BMIGoalChart.tsx
│   │   │   │   │   ├── WeightGoalChart.tsx
│   │   │   │   │   └── GoalCompletionRate.tsx
│   │   │   │   ├── CourseRecord.tsx
│   │   │   │   └── MobileSidebar.tsx
│   │   │   ├── Footer.tsx
│   │   │   ├── Header.tsx
│   │   │   ├── Layout.tsx
│   │   │   ├── DashboardContainer.tsx
│   │   │   ├── DashboardContainerLayout.tsx
│   │   │   ├── DashboardLayout.tsx
│   │   │   ├── Input.tsx
│   │   │   ├── InputButtonGroup.tsx
│   │   │   ├── InputDate.tsx
│   │   │   ├── InputImage.tsx
│   │   │   ├── InputSwitch.tsx
│   │   │   ├── MetaData.tsx
│   │   │   ├── Textarea.tsx
│   │   │   ├── Select.tsx
│   │   │   └── Loading.tsx
│   │   ├── helpers/
│   │   │   ├── turnDateFormat.tsx
│   │   │   ├── turnStringFormat.tsx
│   │   │   └── errInput.tsx
│   │   ├── hooks/
│   │   │   ├── useEditForm.tsx
│   │   │   ├── usePagination.tsx
│   │   │   ├── useShowModal.tsx
│   │   │   ├── useUploadFile.tsx
│   │   │   └── useResize.tsx
│   │   └── redux/
│   │       ├── features/
│   │       │   ├── dietary-record/
│   │       │   │   ├── bodyRate.tsx
│   │       │   │   ├── dailyDietary.tsx
│   │       │   │   └── goal.tsx
│   │       │   ├── auth.tsx
│   │       │   ├── loading.tsx
│   │       │   ├── paymentPhases.tsx
│   │       │   ├── registerPhases.tsx
│   │       │   └── showModal.tsx
│   │       ├── service/
│   │       │   ├── apply.tsx
│   │       │   ├── course.tsx
│   │       │   ├── courseRecord.tsx
│   │       │   ├── favorite.tsx
│   │       │   ├── intro.tsx
│   │       │   ├── login.tsx
│   │       │   ├── nutritionistList.tsx
│   │       │   ├── order.tsx
│   │       │   ├── payment.tsx
│   │       │   ├── plan.tsx
│   │       │   ├── profile.tsx
│   │       │   ├── register.tsx
│   │       │   └── updatePassword.tsx
│   │       └── store.tsx
│   ├── modules/
│   │   ├── dashboard/
│   │   │   ├── nutritionist/
│   │   │   │   ├── dietary-record/
│   │   │   │   │   ├── MealEditForm.tsx
│   │   │   │   │   └── MenuEditModal.tsx
│   │   │   │   ├── workshop/
│   │   │   │   │   ├── CourseAddForm.tsx
│   │   │   │   │   ├── CourseBigCard.tsx
│   │   │   │   │   ├── CourseDeleteModal.tsx
│   │   │   │   │   ├── CourseStartModal.tsx
│   │   │   │   │   ├── CourseSaveModal.tsx
│   │   │   │   │   └── NutritionistIntroForm.tsx
│   │   │   │   ├── student-list/
│   │   │   │   │   ├── MealEditForm.tsx
│   │   │   │   │   └── MenuEditModal.tsx
│   │   │   │   ├── NutritionistDropdown.tsx
│   │   │   │   └── NutritionistSidebar.tsx
│   │   │   └── student/
│   │   │       ├── course-list/
│   │   │       │   └── CommentAddModal.tsx
│   │   │       ├── dietary-record/
│   │   │       │   └── BodyRateAddModal.tsx
│   │   │       ├── favorite/
│   │   │       │   └── FavoriteCard.tsx
│   │   │       ├── info/
│   │   │       │   └── StudentInfoForm.tsx
│   │   │       ├── order/
│   │   │       │   └── OrderForm.tsx
│   │   │       ├── update-password/
│   │   │       │   └── UpdatePasswordForm.tsx
│   │   │       ├── StudentDropdown.tsx
│   │   │       └── studentSidebar.tsx
│   │   ├── payment/
│   │   │       ├── PaymentForm.tsx
│   │   │       ├── PaymentFormFailurePhase.tsx
│   │   │       ├── PaymentFormSuccessPhase.tsx
│   │   │       └── PaymentFormSecondPhase.tsx
│   │   ├── register/
│   │   │       ├── RegisterForm.tsx
│   │   │       ├── RegisterFormSecondPhase.tsx
│   │   │       └── RegisterFormSecondThird.tsx
│   │   ├── ApplyModal.tsx
│   │   ├── CourseMiniCard.tsx
│   │   ├── CourseNormalCard.tsx
│   │   ├── NutritionistCard.tsx
│   │   ├── NutritionistIntro.tsx
│   │   ├── NutritionistComment.tsx
│   │   └── LoginForm.tsx
│   ├── pages/
│   │   ├── dashboard/
│   │   │   ├── nutritionist/
│   │   │   │   ├── student-list/
│   │   │   │   │   ├── index.tsx
│   │   │   │   │   └── [studentId].tsx
│   │   │   │   ├── workshop/
│   │   │   │   │   ├── intro/
│   │   │   │   │   │   └── index.tsx
│   │   │   │   │   ├── courses/
│   │   │   │   │   │   └── index.tsx
│   │   │   │   │   └── index.tsx
│   │   │   │   └── index.tsx
│   │   │   └── student/
│   │   │       ├── course-list/
│   │   │       │   ├── index.tsx
│   │   │       │   └── [courseId].tsx
│   │   │       ├── order.tsx (optional)
│   │   │       ├── favorite.tsx (optional)
│   │   │       └── info.tsx
│   │   ├── nutritionist-list/
│   │   │   ├── [nutritionistId].tsx
│   │   │   └── index.tsx
│   │   ├── payment/
│   │   │   ├── failure-order.tsx
│   │   │   ├── success-order.tsx
│   │   │   └── index.tsx
│   │   ├── 404.tsx
│   │   ├── 500.tsx
│   │   ├── apply.tsx
│   │   ├── benefit.tsx
│   │   ├── register.tsx
│   │   ├── login.tsx
│   │   ├── _app.tsx
│   │   ├── _document.tsx
│   │   └── index.tsx
│   ├── styles/
│   │   ├── cusButton.css
│   │   ├── cusInput.css
│   │   ├── cusBackdropFilter.css
│   │   ├── fullCalendar.css
│   │   ├── datePicker.css
│   │   ├── swiper.css
│   │   ├── loading.css
│   │   └── globals.css
│   └── types/
│       └── interface.tsx
├── .eslintrc.json
├── .gitignor
├── next.config.js
├── package.json
├── package-lock.json
├── postcss.config.js
├── tailwind.config.ts
├── README.md
└── tsconfig.json
```

<hr/>

## Git規範 
### Git Commit 規則
| 類型 | 格式 | 說明 |
| :---: | :---: | :--- |
| 新增功能 | feat: | 新增功能 |
| 修改功能 | modify: | 調整現有功能(不是錯誤) |
| 刪除功能 | remove: | 刪除現有功能 |
| 修補錯誤 | fix: | 修正現有功能的錯誤 |
| 重構程式 | refactor: | 重組、優化程式(邏輯不變) |
| 快速修正 | hotfix: | 緊急修正現有功能的錯誤 |
| 樣式相關 | style: | 修改程式碼風格 (不影響程式碼運行的變動) |
| 維護文件 | docs: | 新增/修改文件 |
| 維護資料 | chore: | 更新專案建置、版本…其他 (不影響程式碼運行的變動) |

### Git Branch 規則
| 類型 | 格式 |
| :---: | :---: |
| 新增功能 | feat/ |
| 修改功能 | fix/ |
| 刪除功能 | hotfix/ |

<hr/>

## 技術規格 
### 後端
- C#
- MS SQL
- Swagger
- Postman
- ASP.Net
- LINQ
- AWS

### 前端
- Next
- React
- TypeScript
- React Hook Form
- RTK (Query)
- TailwindCSS

