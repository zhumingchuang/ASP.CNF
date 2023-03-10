function changeSrcCode() {
    $("#captchaPic").attr("src", $("#captchaPic").attr("src") + 1);// 取得img属性 得到src地址给它+1 是为了每次变换验证码
};
layui.use(['jquery', 'form', 'common'], function () {
    var form = layui.form,
        $ = layui.jquery,
        os = layui.common,
        layer = layui.layer;


    function login(options) {
        $.ajax("login", {
            contentType: "application/json",
            dataType: 'json',
            type: "post",
            data: JSON.stringify(options),
            success: function (res) {
                loginTextChange(true);
                if (res.statusCode == 200) {
                    if (res.success) {
                        os.SetSession('globalCurrentUserInfo', res.data);
                        setTimeout(function () {
                            os.success("恭喜您，登录成功");
                            var rurl = os.getUrlParam('returnUrl');
                            if (!rurl) {
                                window.location.href = '/shennius-master.html';
                            }
                            else {
                                window.location.href = "/shennius-master.html#" + rurl;
                            }
                        }, 500);
                        return;
                    } else {
                        if (res.msg.indexOf("已经登录") != -1) {
                            layer.confirm(res.msg, /*{ icon: 3, title: '提示' },*/ {
                                btn: ['继续登录', '取消']
                            }, function (index) {
                                options.confirmLogin = true;
                                layer.close(index);
                                //此处请求后台程序，下方是成功后的前台处理……
                                var index = layer.load(0, { shade: [0.7, '#393D49'] }, { shadeClose: true });
                                login(options);
                            }, function (index) {
                                loginTextChange(false);
                                layer.close(index);
                            });
                            return false;
                        }
                        if (res.data.menuAuthOutputs == null || res.data.menuAuthOutputs.length <= 0) {
                            os.error("不好意思，该用户当前没有权限。请联系系统管理员分配权限！");
                            loginTextChange(false);
                            return;
                        }
                    }
                } else {
                    os.error(res.msg);
                    loginTextChange(false);
                }
            },
            error: function (e) {
                if (e != null) {
                    console.log("异常:" + e);
                    var res = JSON.parse(e.responseText);
                    if (res.statusCode == 400) {
                        toastr.error(res.msg);
                        return;
                    }
                }
                this.error('连接异常，请稍后重试！');
                loginTextChange(false);
                return;
            }
        });    
};
    // 登录过期的时候，跳出ifram框架
    if (top.location != self.location) top.location = self.location;

    // 进行登录操作
    form.on('submit(login)', function (data) {
        if (data.field.captcha == '') {
            layer.msg('验证码不能为空');
            return false;
        }
        var crypt = new JSEncrypt();
        crypt.setPrivateKey(data.field.privateKey);
        var enc = crypt.encrypt(data.field.password);
        data.field.password = enc;
        //console.log("password:" + data.field.password)
        data.field.confirmLogin = false;
        login(data.field);
        return false;
    });

    function loginTextChange(isSuccessing) {
        if (isSuccessing) {
            $("#btnlogin").text("正在登录中...");
            $("#btnlogin").attr('disabled', 'disabled');
        } else {
            $("#btnlogin").text("立即登录");
            $("#btnlogin").attr('disabled', false);
        }
    };

    $(window).resize(
        bodysize
    );
    bodysize();
    function bodysize() {
        $("body").height($(window).height());
    }
});