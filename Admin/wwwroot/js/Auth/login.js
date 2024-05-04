(function IIFE() {
    const HOME_PAGE = '/'
    const api = { login: '/api/Auth/Login' }
    const apiCaller = { login: (loginQuery) => httpPost(api.login, loginQuery) }

    const authLoginVue = Vue.createApp({
        data() {
            return {
                login: {
                    userName: '',
                    password: ''
                }
            }
        },
        methods: {
            loginBtn() {
                handleLogin({ ...this.login })
            }
        },
        mounted() {
            localStorage.removeItem('token');
        }
    })
    authLoginVue.mount('#authLogin')
    function handleLogin(loginQuery) {
        apiCaller.login(loginQuery)
            .then((res) => {
                console.log(res)
                if (res.isSuccess) {
                    const { token, expireTime } = res.body
                    setToken(token, expireTime)
                    redirectToHome()
                }
            })
            .catch(err => {
                console.error(err)
            })
    }

    function setToken(token, expireTime) {
        localStorage.setItem('token', token);
        localStorage.setItem('expireTime', `${expireTime}`);
    }

    function redirectToHome() {
        window.location.href = HOME_PAGE
    }
})()