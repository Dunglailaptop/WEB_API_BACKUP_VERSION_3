
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Payment Success</title>
    <!-- Định dạng CSS hoặc các tập tin tài nguyên khác -->
    <style>
 body {
  background: #999;
}

.container {
  max-width: 780px;
  margin: 30px auto;
  overflow: hidden;
}

.printer-top {
  z-index: 1;
  border: 6px solid #666666;
  height: 6px;
  border-bottom: 0;
  border-radius: 6px 6px 0 0;
  background: #333333;
}

.printer-bottom {
  z-index: 0;
  border: 6px solid #666666;
  height: 6px;
  border-top: 0;
  border-radius: 0 0 6px 6px;
  background: #333333;
}

.paper-container {
  position: relative;
  overflow: hidden;
  height: 1800px;
}

.paper {
  background: #ffffff;
  height: 1800px;
  position: absolute;
  z-index: 2;
  margin: 0 12px;
  margin-top: -12px;
  animation: print 1800ms cubic-bezier(0.68, -0.55, 0.265, 0.9) infinite;
  -moz-animation: print 1800ms cubic-bezier(0.68, -0.55, 0.265, 0.9) infinite;
}

.main-contents {
  margin: 0 12px;
  padding: 24px;
}


.jagged-edge {
  position: relative;
  height: 20px;
  width: 100%;
  margin-top: -1px;
}

.jagged-edge:after {
  content: "";
  display: block;
  position: absolute;
  left: 0;
  right: 0;
  height: 20px;
  background: linear-gradient( 45deg, transparent 33.333%, #ffffff 33.333%, #ffffff 66.667%, transparent 66.667%), linear-gradient( -45deg, transparent 33.333%, #ffffff 33.333%, #ffffff 66.667%, transparent 66.667%);
  background-size: 16px 40px;
  background-position: 0 -20px;
}

.success-icon {
  text-align: center;
  font-size: 48px;
  height: 72px;
  background: #359d00;
  border-radius: 50%;
  width: 72px;
  height: 72px;
  margin: 16px auto;
  color: #fff;
}

.success-title {
  font-size: 120px;
  text-align: center;
  color: #666;
  font-weight: bold;
  margin-bottom: 16px;
}

.success-description {
  font-size: 48px;
  line-height: 21px;
  color: #999;
  text-align: center;
  margin-bottom: 24px;
}

.order-details {
  text-align: center;
  color: #333;
  font-weight: bold;
  .order-number-label {
    font-size: 120px;
    margin-bottom: 8px;
  }
  .order-number {
    border-top: 1px solid #ccc;
    border-bottom: 1px solid #ccc;
    line-height: 48px;
    font-size: 120px;
    padding: 8px 0;
    margin-bottom: 24px;
  }
}

.order-footer {
  text-align: center;
  line-height: 18px;
  font-size: 50px;
  margin-bottom: 8px;
  font-weight: bold;
  color: #999;
}


    </style>
</head>
<body>
    <div class="container">
        <div class="printer-top"></div>
          
        <div class="paper-container">
          <div class="printer-bottom"></div>
      
          <div class="paper">
            <div class="main-contents">
              <div class="success-icon">&#10004;</div>
              <div class="success-title">
                PAYMENT SUCCESSFULL
              </div>
              <div class="order-details">
                <div style="font-size: 120px;" class="order-number-label">Order Number:</div>
                <div style="font-size: 120px; color:red" class="order-number"> {{name}}</div>
              </div>
              <div class="order-footer">Thank you!</div>
            </div>
            <div class="jagged-edge"></div>
          </div>
        </div>
      </div>
</body>
</html>
